using System.ComponentModel.DataAnnotations;

namespace UserSystem.Api.Dtos;

public class RegisterUserRequest
{
    [EmailAddress] public string Email { get; set; }

    [Required(ErrorMessage = "Mobile no. is required")]
    [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
    public string Phone { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be more than 6 characters")]
    public string Password { get; set; }
}