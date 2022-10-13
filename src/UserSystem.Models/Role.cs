using System.Text.Json.Serialization;
using UserSystem.Models.Enums;

namespace UserSystem.Models;

public class Role
{
    public UserRole Id { get; set; }
    [JsonIgnore] public List<User> Users { get; set; }
}