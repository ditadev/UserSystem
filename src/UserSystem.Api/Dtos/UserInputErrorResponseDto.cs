using FluentValidation.Results;
using UserSystem.Models.Enums;
using UserSystem.Models.Records;

namespace UserSystem.Api.Dtos;

public class UserInputErrorResponseDto : ResponseDto<List<ValidationError>?>
{
    public UserInputErrorResponseDto(ValidationResult validationResult, string message = "Invalid Input") : base(
        AppFaultCode.UserInputError,
        message,
        validationResult.Errors.Select(x => new ValidationError(x.ErrorMessage, x.PropertyName)).ToList())
    {
    }

    public UserInputErrorResponseDto(string message = "Invalid Input", params ValidationError[]? validationErrors) :
        base(AppFaultCode.UserInputError,
            message, validationErrors?.ToList())
    {
    }
}