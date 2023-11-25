using System.Net.Security;
using System.Security.Claims;
using System.Security.Principal;
using Instagrad.Domain;
using Instagrad.Domain.Abstractions;
using Instagrad.WebApi.Controllers.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Instagrad.Infrastructure.Repositories;

namespace Instagrad.WebApi.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("singin")]
    public async Task<IActionResult> SingIn([FromForm] SingInRequest request)
    {

        var user = _userRepository.GetAll().FirstOrDefault(u =>
            u.CheckCredentials(request.Login, request.Password));

        if (user is not null)
        {
            await Authenticate(request.Login);

            return Ok(user);
        }



        return Unauthorized(request);
    }

    [HttpPost("singup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SingUp([FromForm] SingUpRequest request)
    {
        //TODO: make validation
        if (string.IsNullOrEmpty(request.Login)
            || string.IsNullOrEmpty(request.Password)
            || request.Password.Length < 6)
        {
            return BadRequest();
        }

        var user = new User(request.Login, request.Password);

        _userRepository.Add(user);

        return Created(user.Login, user);
    }



    private async Task Authenticate(string login)
    {

        var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            };

        var claimsId = new ClaimsIdentity(claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsId));

    }
}