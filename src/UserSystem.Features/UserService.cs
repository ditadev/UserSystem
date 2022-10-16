using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using UserService.Persistence;
using UserSystem.Models;
using UserSystem.Models.Enums;
using Role = UserSystem.Models.Role;

namespace UserSystem.Features;

public class UserService : IUserService
{
    private static readonly Random GenerateRandomToken = new();
    private readonly AppSettings _appSettings;
    private readonly IDatabase _database;
    private readonly DataContext _dataContext;
    private readonly IEmailService _emailService;

    public UserService(
        DataContext dataContext,
        AppSettings appSettings,
        IConnectionMultiplexer redis,
        IEmailService emailService)
    {
        _dataContext = dataContext;
        _appSettings = appSettings;
        _emailService = emailService;
        _database = redis.GetDatabase();
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

    public string CreateRandomToken()
    {
        int CreateRandomNumber(int min, int max)
        {
            lock (GenerateRandomToken)
            {
                return GenerateRandomToken.Next(min, max);
            }
        }

        return CreateRandomNumber(0, 1000000).ToString("D6");
    }

    public async Task CreateUser(User user)
    {
        user.Roles = new List<Role> { await _dataContext.Roles.SingleAsync(x => x.Id == UserRole.User) };
        var token = CreateRandomToken();

        await _database.StringSetAsync($"email_verification_otp:{user.EmailAddress}",
            token, TimeSpan.FromMinutes(20));

        _emailService.Send("to_address@example.com", "Verification Token", token);

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<User> UpdateUser(User user)
    {
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> VerifyUser(string emailAddress, string token)
    {
        var tokenCheck = await _database.StringGetAsync($"email_verification_otp:{emailAddress}");

        if (token == tokenCheck)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.EmailAddress == emailAddress);
            if (user != null)
                user.VerifiedAt = DateTime.UtcNow;

            await _dataContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public Task<bool> VerifyPassword(User user, string password)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
    }

    public async Task<bool> ForgotPassword(string emailAddress)
    {
        var user = await GetUserByEmailAddress(emailAddress);
        if (user == null) return false;

        var token = CreateRandomToken();
        await _database.StringSetAsync($"email_reset_otp:{emailAddress}",
            token, TimeSpan.FromMinutes(20));
        _emailService.Send("to_address@example.com", "Reset Token", token);
        return true;
    }

    public async Task<bool> ResetPassword(string emailAddress, string token, string password)
    {
        var user = await GetUserByEmailAddress(emailAddress);
        var tokenCheck = await _database.StringGetAsync($"email_reset_otp:{emailAddress}");
        if (tokenCheck != token) return false;

        var passwordHash = await CreatePasswordHash(password);
        if (user != null)
        {
            user.PasswordHash = passwordHash;
            await UpdateUser(user);
        }

        return true;
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