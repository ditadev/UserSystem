using UserSystem.Models.Enums;

namespace UserSystem.Api.Dtos;

public class UserNotAuthenticatedErrorResponseDto : ResponseDto<List<dynamic>>
{
    public UserNotAuthenticatedErrorResponseDto(string message = "Access Denied") : base(
        AppFaultCode.UserNotAuthenticated,
        message, null)
    {
    }
}