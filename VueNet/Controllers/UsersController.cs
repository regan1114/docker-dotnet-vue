using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueNet.Authorization;
using VueNet.Models;
using VueNet.Services;

namespace VueNet.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(AuthenticateRequest model)
    {
        var response = _userService.Login(model, IpAddress());
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    public IActionResult RefreshToken(UpdateTokenModel model)
    {
        var response = _userService.RefreshToken(model.Token);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return BadRequest(new { message = "無使用者" });
        }
        return Ok(user);
    }

    private string IpAddress()
    {
        // get source ip address for the current request
        return Request.Headers.ContainsKey("X-Forwarded-For") ?
            Request.Headers["X-Forwarded-For"] :
            HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}
