using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Persistence;
using UserSystem.Models;

namespace UserSystem.Features;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    public async Task<string> CreatePasswordHash(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public Task<string> CreateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public Task<bool> VerifyPasswordHash(string password, string passwordHash)
    {
        return Task.FromResult((BCrypt.Net.BCrypt.Verify(password, passwordHash)));
    }

    public async Task<User?> GetUserById(long id)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }
}