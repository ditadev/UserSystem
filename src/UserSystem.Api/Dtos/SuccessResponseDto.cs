using UserSystem.Models.Enums;

namespace UserSystem.Api.Dtos;

public class SuccessResponseDto<TResultDto> : ResponseDto<TResultDto>
{
    public SuccessResponseDto(TResultDto? data, string message = "Success") : base(AppFaultCode.Success, message, data)
    {
    }
}