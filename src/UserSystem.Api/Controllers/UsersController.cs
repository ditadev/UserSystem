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
    private readonly IDatabase _database;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;

    public UsersController(IConnectionMultiplexer redis,IUserService userService, IEmailService emailService,AppSettings appSettings)
    {
        _database = redis.GetDatabase();
        _userService = userService;
        _emailService = emailService;
        _appSettings = appSettings;
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
        var token = await _userService.CreateRandomToken();
        await _database.StringSetAsync($"email_verification_otp:{request.EmailAddress}",
            token,new TimeSpan(0,20,0));
        _emailService.Send("to_address@example.com","Verification Token",token);
        
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
    public async Task<ActionResult> VerifyUser(string emailAddress,string token)
    {
        var tokenCheck = await _database.StringGetAsync($"email_verification_otp:{emailAddress}");
        if (token==tokenCheck)
        {
            await _userService.VerifyUser(emailAddress,token);
            return Ok("Verification Successful :)");
        }

        return BadRequest("Invalid OTP :(");
    }   
    
    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string emailAddress)
    {
        var user = await _userService.GetUserByEmailAddress(emailAddress);
        if (user == null) return BadRequest("User not found :(");

        var token = await _userService.CreateRandomToken();
        await _database.StringSetAsync($"email_reset_otp:{emailAddress}",
            token,new TimeSpan(0,20,0));
        _emailService.Send("to_address@example.com","Reset Token",token);
       

        return Ok("You may now reset your password :)");
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword([FromForm]ResetPasswordRequest request)
    {
        var user = await _userService.GetUserByEmailAddress(request.emailAddress);
        var tokenCheck = await _database.StringGetAsync($"email_reset_otp:{request.emailAddress}");
        if (tokenCheck != request.Token) return BadRequest("Invalid Token :(");

        var passwordHash = await _userService.CreatePasswordHash(request.Password);
        user.PasswordHash = passwordHash;

        await _userService.UpdateUser(user);

        return Ok("Password successfully reset :)");
    }
}