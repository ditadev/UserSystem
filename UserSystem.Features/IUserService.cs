using UserSystem.Models;

namespace UserSystem.Features;

public interface IUserService
{
    public Task<string> CreatePasswordHash(string password);
    public Task<string> CreateJwt(User user);
    public Task<bool> VerifyPassword(string password, User user);
    public Task<User?> GetUserById(long id);
    public Task<User?> GetUserByEmail(string email);
    public Task<User?> CreateUser(User user);
    
}