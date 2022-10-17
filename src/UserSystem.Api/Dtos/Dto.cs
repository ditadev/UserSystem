namespace UserSystem.Api.Dtos;

public abstract class Dto<TResultDto>
{
    public AppFaultCode Code { get; set; }
    public string Message { get; set; }
    public TResultDto Data { get; set; }

    protected Dto(AppFaultCode code, string message, TResultDto data)
    {
        Code = code;
        Message = message;
        Data = data;
    }
}