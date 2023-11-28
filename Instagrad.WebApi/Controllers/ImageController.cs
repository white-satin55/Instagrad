using System.IO;
using System.Security.Claims;

using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using Instagrad.Infrastructure.Repositories;
using Instagrad.WebApi.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using MimeMapping;
using MimeTypes;

namespace Instagrad.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly string _localStoragePath = "..\\..\\..\\Storage\\Images";
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;
    private User _currentUser;

    public ImageController(IImageRepository imageRepository,
        IUserRepository userRepository)
    {
        _imageRepository = imageRepository;
        _userRepository = userRepository;
        _currentUser = _userRepository.GetById(User.Identity.Name);
    }

    #region Image querries handling
    private ActionResult UserHasAccess(string userLogin)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        var user = _userRepository
            .GetAll()
            .FirstOrDefault(u => u.Login.Equals(userLogin));

        if (user == null)
        {
            return NotFound();
        }

        if (!user.Login.Equals(User.Identity.Name)
            && !user.Friends.Any(u => u.Login.Equals(User.Identity.Name)))
        {
            return Forbid();
        }

        return Ok();
    }

    [HttpGet]
    [Route("{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<string>> GetUserImagesIdList([FromRoute] string userLogin)
    {
        var accessResult = UserHasAccess(_currentUser.Login);
        
        if (accessResult is not OkResult)
        {
            return accessResult;
        }

        var imageIdList = _imageRepository
            .GetAll()
            .Where(im => im.PublisherLogin.Equals(userLogin))
            .Select(im => im.Id.ToString())
            .ToList();

        return Ok(imageIdList);
    }

    [HttpGet]
    [Route("{userLogin}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Stream> GetImageById([FromRoute] string id)
    {
        var accessResult = UserHasAccess(_currentUser.Login);

        if (accessResult is not OkResult)
        {
            return accessResult;
        }

        try
        {
            Image image = _imageRepository
                .GetAll()
                .First(im => id.Equals(im.Id.ToString()));

            string imageExtension = MimeTypeMap.GetExtension(image.MediaType);

            var file = System.IO.File.OpenRead($"{_localStoragePath}\\{image.Id}{imageExtension}");

            return File(file, image.MediaType);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
    #endregion

    #region Image commands handling
    [HttpPost]
    [Route("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        string extension = Path.GetExtension(file.FileName);

        string fileId = (Guid.NewGuid()).ToString();
        string path = _localStoragePath + '\\' + fileId + extension;

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            await file.CopyToAsync(fileStream);
        }

        var image = new Image
        {
            Id = Guid.Parse(fileId),
            PublisherLogin = _currentUser.Login,
            MediaType = MimeTypeMap.GetMimeType(extension)
        };

        _imageRepository.Add(image);

        return Created($"/images", GetImageById(image.Id.ToString()));
    }
    #endregion
}