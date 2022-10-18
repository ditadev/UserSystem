using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UserSystem.Api.Attributes;
using UserSystem.Api.Dtos;
using UserSystem.Features;
using UserSystem.Models;
using UserSystem.Models.Enums;
using UserSystem.Models.Helper;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(UserRole.Default)]
public class UserController : AbstractController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(UserRole.User)]
    public async Task<ActionResult<UserDto>> Get()
    {
        var userId = GetContextUserId();
        var user = await _userService.GetUserById(userId);
    
        if (user == null) return BadRequest("User Not Found :(");
    
        return Ok(new UserDto(AppFaultCode.Success,"Success",user));
    }
}