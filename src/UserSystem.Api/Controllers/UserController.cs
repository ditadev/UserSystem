using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UserSystem.Api.Attributes;
using UserSystem.Api.Dtos;
using UserSystem.Features;
using UserSystem.Models;
using UserSystem.Models.Enums;
using UserSystem.Models.Helper;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(UserInputErrorResponseDto), 400)]
[ProducesResponseType(typeof(UserNotAuthenticatedErrorResponseDto), 401)]
[ProducesResponseType(typeof(UserNotPrivilegedResponseDto), 403)]
[ProducesResponseType(typeof(ServerErrorResponseDto), 500)]
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
    [ProducesResponseType(typeof(UserResponseDto), 200)]
    public async Task<ActionResult<UserResponseDto>> Get()
    {
        var userId = GetContextUserId();
        var user = await _userService.GetUserById(userId);

        if (user == null) return BadRequest(new UserInputErrorResponseDto("User Not Found :("));

        return Ok(new UserResponseDto(user));
    }
}
