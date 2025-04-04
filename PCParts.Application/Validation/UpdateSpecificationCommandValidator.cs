using FluentValidation;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Domain.Enum;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class UpdateSpecificationCommandValidator : AbstractValidator<UpdateSpecificationCommand>
{
    public UpdateSpecificationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.Value)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong)
            .Must((command, value) =>
                ValidationHelper.IsValueValid((SpecificationDataType)command.Type, command.Value) ||
                string.IsNullOrEmpty(value))
            .WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
        RuleFor(x => x.Type)
            .IsInEnum().WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Value) == string.IsNullOrEmpty(x.Name))
            .WithErrorCode(ValidationErrorCode.Empty);
    }
}