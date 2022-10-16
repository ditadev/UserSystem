using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
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
            PasswordHash = passwordHash
        });
        return Ok("Successfully Created :)");
    }

    [HttpPost]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest request)
    {
        var user = await _userService.GetUserByEmailAddress(request.EmailAddress);
        if (user?.VerifiedAt == null) return BadRequest("Not verified :(");

        if (user == null || !await _userService.VerifyPassword(user, request.Password))
            return BadRequest("Incorrect Username/Password :(");

        return Ok(new JwtDto
        {
            AccessToken = await _userService.CreateJwt(user)
        });
    }

    [HttpPost]
    public async Task<ActionResult> VerifyUser(string emailAddress, string token)
    {
        if (await _userService.VerifyUser(emailAddress, token) == false)
            return BadRequest("Invalid OTP :(");
        
        return Ok("User Verified :)");
    }

    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string emailAddress)
    {
        if (await _userService.ForgotPassword(emailAddress) == false)
            return BadRequest("User not found :(");

        return Ok("You may now reset your password :)");
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword([FromForm] ResetPasswordRequest request)
    {
        if (await _userService.ResetPassword(request.emailAddress, request.Token, request.Password) == false)
            return BadRequest("Invalid OTP :(");
        return Ok("Password successfully reset :)");
    }
}