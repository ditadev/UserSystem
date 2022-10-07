using System.Text.Json.Serialization;

namespace UserSystem.Models;

public class User
{
    public long Id { get; set; }
    public string EmailAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; }
}