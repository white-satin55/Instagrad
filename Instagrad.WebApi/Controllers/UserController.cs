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
        _currentUser = _userRepository.GetById(User?.Identity?.Name);
    }
    
    #region Friendship request handling
    [HttpGet]
    [Route("send/{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
    public async Task<IActionResult> SendFriendshipRequest(string userLogin)
    {
        var receiver = await _userRepository.GetByIdAsync(userLogin);

        receiver.ReceiveFriendshipRequest(_currentUser);

        return Ok();
    }

    [HttpGet]
    [Route("accept/{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AcceptFriendshipRequest(string userLogin)
    {
        _currentUser.AcceptFriendshipRequest(await _userRepository.GetByIdAsync(userLogin));

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
            (await _userRepository
            .GetByIdAsync(userLogin))
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
            (await _userRepository
            .GetByIdAsync(userLogin))
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
            (await _userRepository
            .GetByIdAsync(userLogin))
            .OutgoingFriendshipRequests
            .Select(f => f.Login)
            .ToList());
    }
    #endregion  
}