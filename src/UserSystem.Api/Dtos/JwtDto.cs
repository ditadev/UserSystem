namespace UserSystem.Api.Dtos;

public class JwtDto : Dto<JwtDto.Data>
{
    public class Data
    {
        public string AccessToken { get; set; }
    }

    public JwtDto(AppFaultCode code, string message, Data data) : base(code, message, data)
    {
    }
}