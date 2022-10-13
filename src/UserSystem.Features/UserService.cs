using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Persistence;
using UserSystem.Models;
using UserSystem.Models.Enums;

namespace UserSystem.Features;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext, AppSettings appSettings)
    {
        _dataContext = dataContext;
        _appSettings = appSettings;
    }

    public async Task<string> CreatePasswordHash(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public Task<string> CreateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var claims = new List<Claim> { new("sub", user.Id.ToString()), new("role", UserRole.Default.ToString()) };

        claims.AddRange(user.Roles.Select(role => new Claim("role", role.Id.ToString())));

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            )));
    }

    public async Task CreateUser(User user)
    {
        user.Roles = new List<Role> { await _dataContext.Roles.SingleAsync(x => x.Id == UserRole.User) };
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
    }

    public Task<bool> VerifyPassword(User user, string password)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
    }

    public Task<User?> GetUserById(long id)
    {
        return _dataContext.Users.Include(x => x.Roles).SingleOrDefaultAsync(u => u.Id == id);
    }

    public Task<User?> GetUserByEmailAddress(string emailAddress)
    {
        return _dataContext.Users.Include(x => x.Roles).SingleOrDefaultAsync(u => u.EmailAddress == emailAddress);
    }
}