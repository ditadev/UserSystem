namespace UserSystem.Features;

public interface IEmailService
{
    void Send(string to, string subject, string html);
}