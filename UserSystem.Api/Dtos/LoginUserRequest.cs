using System.ComponentModel.DataAnnotations;

namespace UserSystem.Api.Dtos;

public class LoginUserRequest
{
    [Required] [EmailAddress] public string EmailAddress { get; set; }
    [Required] public string Password { get; set; }
}