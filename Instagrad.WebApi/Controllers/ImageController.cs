using System.IO;
using System.Security.Claims;
using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using Instagrad.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MimeMapping;

namespace Instagrad.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("images")]
public class ImageController : ControllerBase
{
    private readonly string _localStoragePath = "D:\\Projects\\Instagrad\\Storage\\Images";
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;    

    public ImageController(IImageRepository imageRepository,
        IUserRepository userRepository)
    {        
        _imageRepository = imageRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Stream>> GetImages([FromRoute] string userLogin)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        //TODO: make validator
        #region Validation
        var user = _userRepository.GetAll().FirstOrDefault(u => u.Login.Equals(userLogin));

        if (user == null)
        {
            return NotFound();
        }

        if (!user.Login.Equals(User.Identity.Name)
            && !user.Friends.Any(u => u.Login.Equals(User.Identity.Name)))
        {
            return Forbid();
        }
        #endregion

        var images = _userRepository.GetAll()
            .First(u => u.Login.Equals(userLogin))
            .Images
            .Select(im => GetImageContent(im.Id.ToString()))
            .ToList();

        return Ok(images);
    }

    private ActionResult<Stream> GetImageContent(string id)
    {
        using var file = new FileStream($"{_localStoragePath}\\{id}", FileMode.Open);

        //TODO: returns any image extension
        return File(file, "image/jpeg");
    }

    [HttpPost]
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        string userName = User.Identity.Name;

        string extension = Path.GetExtension(file.FileName);

        string fileId = (Guid.NewGuid()).ToString();
        string path = _localStoragePath + '\\' + fileId + extension;        

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            await file.CopyToAsync(fileStream);
        }

        var image = new Image
        {
            Id = Guid.NewGuid(),
            PublisherLogin = _userRepository.GetAll().First(u =>
                u.Login.Equals(User.Identity.Name)).Login
        };

        _imageRepository.Add(image);

        return /*Created(path, image)*/Ok();
    }
}