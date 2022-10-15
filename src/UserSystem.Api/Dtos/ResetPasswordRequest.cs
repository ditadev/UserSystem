using System.ComponentModel.DataAnnotations;

namespace UserSystem.Api.Dtos;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public string emailAddress { get; set; }
    [Required]public string Token { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }

    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
}