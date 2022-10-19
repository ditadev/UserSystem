using UserSystem.Models.Enums;

namespace UserSystem.Api.Dtos;

public abstract class ResponseDto<TResultDto>
{
    protected ResponseDto(AppFaultCode code, string message, TResultDto? data)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public AppFaultCode Code { get; set; }
    public string Message { get; set; }
    public TResultDto? Data { get; set; }
}