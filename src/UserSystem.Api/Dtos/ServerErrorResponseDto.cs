using UserSystem.Models.Enums;

namespace UserSystem.Api.Dtos;

public class ServerErrorResponseDto : ResponseDto<List<dynamic>>
{
    public ServerErrorResponseDto(string message = "Server Error") : base(AppFaultCode.ServerError, message, null)
    {
    }
}