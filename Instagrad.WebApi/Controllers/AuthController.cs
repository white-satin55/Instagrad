using Instagrad.Domain;
using Instagrad.WebApi.Controllers.Requests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Instagrad.Infrastructure.Repositories;

namespace Instagrad.WebApi.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("singin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SingIn([FromForm] SingInRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.Login);

        if (!user.CheckCredentials(request.Login, request.Password))
        {
            return NotFound();
        }

        await Authenticate(request.Login);

        return Ok(user);
    }

    [HttpPost]
    [Route("singup")]
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
