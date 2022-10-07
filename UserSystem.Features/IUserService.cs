using UserSystem.Models;

namespace UserSystem.Features;

public interface IUserService
{
    public Task<string> CreatePasswordHash(string password);
    public Task<string> CreateJwt(User user);
    public Task<bool> VerifyPasswordHash(string password, string passwordHash);
    public Task<User?> GetUserById(long id);
}