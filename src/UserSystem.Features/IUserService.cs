using UserSystem.Models;

namespace UserSystem.Features;

public interface IUserService
{
    public Task<string> CreatePasswordHash(string password);
    public Task<string> CreateJwt(User user);
    public Task<string> CreateRandomToken();
    public Task CreateUser(User user);
    public Task<User> UpdateUser(User user);
    public Task VerifyUser(string emailAddress,string token);
    public Task<bool> VerifyPassword(User user, string password);
    public Task<User?> GetUserById(long id);
    public Task<User?> GetUserByEmailAddress(string emailAddress);
}