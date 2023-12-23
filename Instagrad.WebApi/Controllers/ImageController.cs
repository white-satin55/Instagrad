using Instagrad.Domain;
using Instagrad.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MimeTypes;

namespace Instagrad.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("images")]
public class ImageController : ControllerBase
{
    private readonly string _localStoragePath = "..\\..\\Storage\\Images";
    private IImageRepository _imageRepository;
    private IUserRepository _userRepository;    

    //TODO: add user info
    public ImageController(IImageRepository imageRepository,
        IUserRepository userRepository)
    {
        _imageRepository = imageRepository;
        _userRepository = userRepository;        
    }

    #region Image querries handling
    private async Task<ActionResult> HasAccessToUserAsync(string userLogin)
    {
        var user = await _userRepository.GetByIdAsync(userLogin);

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
    public async Task<ActionResult<List<string>>> GetUserImagesIdListAsync([FromRoute] string userLogin)
    {
        var accessResult = await HasAccessToUserAsync(User.Identity.Name);

        if (accessResult is not OkResult)
        {
            return accessResult;
        }

        userLogin ??= User.Identity.Name;

        var imageIdList = (await _imageRepository
            .GetAllAsync())
            .Where(im => im.PublisherLogin.Equals(userLogin))
            .Select(im => im.GetEncodedId())
            .ToList();

        return Ok(imageIdList);
    }

    [HttpGet]
    [Route("{userLogin}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Stream>> GetImageByIdAsync([FromRoute] string userLogin, [FromRoute] string id)
    {
        var accessResult = await HasAccessToUserAsync(userLogin);

        if (accessResult is not OkResult)
        {
            return accessResult;
        }

        try
        {            
            Image image = await _imageRepository                
                .GetByIdAsync(id);

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
            PublisherLogin = User.Identity.Name,
            MediaType = MimeTypeMap.GetMimeType(extension)
        };

        _imageRepository.Add(image);

        return Created($"/images/{User.Identity.Name}/{image.Id}", GetImageByIdAsync(User.Identity.Name, image.GetEncodedId()));
    }
    #endregion
}