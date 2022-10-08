using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Persistence;
using UserSystem.Api.Dtos;
using UserSystem.Features;
using UserSystem.Models;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterUserRequest request)
    {
        var user = await _userService.GetUserByEmail(request.Email);
        
        if (user != null) return BadRequest("User Already Exist :(");
        
        var passwordHash = await _userService.CreatePasswordHash(request.Password);
        var newUser = new User
        {
            EmailAddress = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash
        };

        await _userService.CreateUser(newUser);
        return Ok(" Successfully Created :)");
    }

    [HttpPost]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest request)
    {
        var user = await _userService.GetUserByEmail(request.Email);

        if (user == null) return BadRequest("User not found :(");

        if (!await _userService.VerifyPassword(request.Password, user))
            return BadRequest("Incorrect Username/Password :(");

        return Ok(new JwtDto
        {
            AccessToken = await _userService.CreateJwt(user)
        });
    }
}