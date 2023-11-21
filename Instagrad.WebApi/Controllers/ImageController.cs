﻿using System.IO;
using System.Security.Claims;

using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using Instagrad.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

using MimeMapping;
using MimeTypes;

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

    //TODO: fix returning of image list
    [HttpGet]
    [Route("{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<ActionResult<Stream>>> GetImages([FromRoute] string userLogin)
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

        int imagesCount = _imageRepository
            .GetAll()
            .Count(im => im.PublisherLogin.Equals(userLogin));

        var images = new List<ActionResult<Stream>>();

        for (int i = 0; i < imagesCount; i++)
        {
            var image = GetImage(userLogin, i);
            images.Add(image);
        }

        return Ok(images);
    }

    [HttpGet]
    [Route("{userLogin}/{index}")]
    public ActionResult<Stream> GetImage([FromRoute] string userLogin, 
        [FromRoute] int index)
    {
        try
        {
            Image image = _imageRepository
                .GetAll()
                .Where(im => im.PublisherLogin.Equals(userLogin))
                .ElementAt(index);

            string imageExtension = MimeTypeMap.GetExtension(image.MediaType);

            var file = System.IO.File.OpenRead($"{_localStoragePath}\\{image.Id}{imageExtension}");

            return File(file, image.MediaType);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
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
            Id = Guid.Parse(fileId),
            PublisherLogin = _userRepository.GetAll().First(u =>
                u.Login.Equals(User.Identity.Name)).Login,
            MediaType = MimeTypeMap.GetMimeType(extension)
        };

        _imageRepository.Add(image);

        return /*Created(path, image)*/Ok();
    }
}