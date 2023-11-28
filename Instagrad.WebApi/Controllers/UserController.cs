using System.Security.Claims;
using Instagrad.Domain;
using Instagrad.WebApi.Controllers.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Instagrad.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Instagrad.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;
    private User _currentUser; 

    public UserController(IUserRepository userRepository)
    { 
        _userRepository = userRepository;
        _currentUser = _userRepository.GetById(User.Identity.Name);
    }
    
    #region Friendship request handling
    [HttpGet]
    [Route("send/{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SendFriendshipRequest(string userLogin)
    {
        var receiver = _userRepository.GetById(userLogin);

        receiver.ReceiveFriendshipRequest(_currentUser);

        return Ok();
    }

    [HttpGet]
    [Route("accept/{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AcceptFriendshipRequest(string userLogin)
    {
        _currentUser.AcceptFriendshipRequest(_userRepository.GetById(userLogin));

        return Ok();
    }
    #endregion

    //TODO: add correct return of request
    #region User data handling
    [HttpGet]
    [Route("data/{userLogin}/friends")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<string>>> GetFriends(string userLogin)
    {
        return Ok(
            _userRepository
            .GetById(userLogin)
            .Friends
            .Select(f => f.Login)
            .ToList());
    }

    [HttpGet]
    [Route("data/{userLogin}/incoming_requests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<string>>> GetIncomingFriendshipRequests(string userLogin)
    {
        return Ok(
            _userRepository
            .GetById(userLogin)
            .IncomingFriendshipRequests
            .Select(f => f.Login)
            .ToList());
    }

    [HttpGet]
    [Route("data/{userLogin}/outgoing_requests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<string>>> GetOutgoingFriendshipRequests(string userLogin)
    {
        return Ok(
            _userRepository
            .GetById(userLogin)
            .OutgoingFriendshipRequests
            .Select(f => f.Login)
            .ToList());
    }
    #endregion

    #region Authorization
    [HttpPost]
    [Route("singin")]
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
    #endregion
}