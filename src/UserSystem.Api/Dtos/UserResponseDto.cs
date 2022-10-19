using UserSystem.Models;

namespace UserSystem.Api.Dtos;

public class UserResponseDto : SuccessResponseDto<User>
{
    public UserResponseDto(User data, string message = "User") : base(data, message)
    {
    }
}