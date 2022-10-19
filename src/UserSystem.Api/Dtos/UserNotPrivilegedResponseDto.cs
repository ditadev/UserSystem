using UserSystem.Models.Enums;

namespace UserSystem.Api.Dtos;

public class UserNotPrivilegedResponseDto : ResponseDto<List<dynamic>>
{
    public UserNotPrivilegedResponseDto(string message = "Unauthenticated") : base(AppFaultCode.UserLacksPrivileges,
        message, null)
    {
    }
}