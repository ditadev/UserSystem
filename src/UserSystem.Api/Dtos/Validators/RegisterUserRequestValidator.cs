using FluentValidation;

namespace UserSystem.Api.Dtos.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.FirstName).Length(2, 30);
        RuleFor(x => x.EmailAddress).EmailAddress();
        RuleFor(x => x.PhoneNumber).Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Phone number must be in E.164 format");
    }
}