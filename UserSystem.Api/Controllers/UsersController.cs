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
    private readonly DataContext _dataContext;
    private readonly IUserService _userService;

    public UsersController(DataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext;
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterUserRequest request)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        var passwordHash = await _userService.CreatePasswordHash(request.Password);

        if (user != null) return BadRequest("User Already Exist :(");

        var newUser = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.Phone,
            PasswordHash = passwordHash
        };

        _dataContext.Users.Add(newUser);
        await _dataContext.SaveChangesAsync();

        return Ok(" Successfully Created :)");
    }

    [HttpPost]
    public async Task<ActionResult<JwtDto>> Login(LoginUserRequest request)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null) return BadRequest("User not found :(");

        if (!await _userService.VerifyPasswordHash(request.Password, user.PasswordHash))
            return BadRequest("Incorrect Username/Password :(");

        return Ok(new JwtDto
        {
            AccessToken = await _userService.CreateJwt(user)
        });
    }
}