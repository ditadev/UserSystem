using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserSystem.Features;
using UserSystem.Models;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<User>> Get()
    {
        var userId = long.Parse(User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        var user = await _userService.GetUserById(userId);
        
        if (user == null) return BadRequest("User Not Found :(");

        return Ok(user);
    }
}