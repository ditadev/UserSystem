using FluentValidation;

namespace UserSystem.Api.Dtos.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.EmailAddress).EmailAddress();
    }
}