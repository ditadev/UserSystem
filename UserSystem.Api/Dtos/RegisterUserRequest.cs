using System.ComponentModel.DataAnnotations;

namespace UserSystem.Api.Dtos;

public class RegisterUserRequest
{
    [EmailAddress] public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Phone number is required!")]
    [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Please enter valid phone number!")]
    public string PhoneNumber { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }
}