using UserSystem.Models;
using UserSystem.Models.Helper;

namespace UserSystem.Features;

public interface IUserService
{
    public Task<string> CreatePasswordHash(string password);
    public Task<string> CreateJwt(User user);
    public string CreateRandomToken();
    public Task CreateUser(User user);
    public Task<User> UpdateUser(User user);
    public Task<bool> VerifyUser(string emailAddress, string token);
    public Task<bool> VerifyPassword(User user, string password);
    public Task<bool> ForgotPassword(string emailAddress);
    public Task<bool> ResetPassword(string emailAddress, string token, string password);
    public Task<User?> GetUserById(ulong id);
    public Task<PagedList<User>> GetAllUsers(PageParameters pageParameters);
    public Task<User?> GetUserByEmailAddress(string emailAddress);
    public Task<User?> GetUserByPhoneNumber(string phoneNumber);
}