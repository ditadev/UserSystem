using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserSystem.Features;
using UserSystem.Models;
using UserSystem.Models.Enums;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : AbstractController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Attributes.Authorize(UserRole.User)]
    public async Task<ActionResult<User>> Get()
    {
        var userId = GetContextUserId();
        var user = await _userService.GetUserById(userId);

        if (user == null) return BadRequest("User Not Found :(");

        return Ok(user);
    }
}