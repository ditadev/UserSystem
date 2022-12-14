using System.Text.Json.Serialization;

namespace UserSystem.Models;

public class User
{
    public ulong Id { get; set; }
    public string EmailAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Role> Roles { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; }
    [JsonIgnore] public DateTime? VerifiedAt { get; set; }
}