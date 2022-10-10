using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserSystem.Api.Attributes;
using UserSystem.Features;
using UserSystem.Models;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
public class UserController : AbstractController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Role.User)]
    public async Task<ActionResult<User>> Get()
    {
        var userId = GetContextUserId();
        var user = await _userService.GetUserById(userId);
        
        if (user == null) return BadRequest("User Not Found :(");

        return Ok(user);
    }
}