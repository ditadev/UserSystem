using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Persistence;
using UserSystem.Api.Dtos;
using UserSystem.Features;
using UserSystem.Models;

namespace UserSystem.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : AbstractController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterUserRequest request)
    {
        var user = await _userService.GetUserByEmailAddress(request.EmailAddress);

        if (user != null) return BadRequest("User Already Exists :(");
        
        var passwordHash = await _userService.CreatePasswordHash(request.Password);

        await _userService.CreateUser(new User
        {
            EmailAddress = request.EmailAddress,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash,
            Roles = new List<Role> { Role.User }
        });

        return Ok("Successfully Created :)");
    }

    [HttpPost]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest request)
    {
        var user = await _userService.GetUserByEmailAddress(request.EmailAddress);
        
        if (user == null || !await _userService.VerifyPassword(user, request.Password))
            return BadRequest("Incorrect Username/Password :(");

        return Ok(new JwtDto
        {
            AccessToken = await _userService.CreateJwt(user)
        });
    }
}