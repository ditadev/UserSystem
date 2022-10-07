using UserSystem.Models;

namespace UserSystem.Features;

public interface IUserService
{
    public Task<string> CreatePasswordHash(string password);
    public Task<string> CreateJwt(User user);
    public Task CreateUser(User user);
    public Task<bool> VerifyPassword(User user, string password);
    public Task<User?> GetUserById(long id);
    public Task<User?> GetUserByEmailAddress(string emailAddress);
}