using UserSystem.Models;

namespace UserSystem.Api.Dtos;

public class UserDto:Dto<User>
{
    public UserDto(AppFaultCode code, string message, User data) : base(code, message, data)
    {
    }
}