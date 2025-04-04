using FluentValidation;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class CreateSpecificationCommandValidator : AbstractValidator<CreateSpecificationCommand>
{
    public CreateSpecificationCommandValidator()
    {
        RuleFor(x => x.ComponentId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.Value)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong)
            .Must((command, value) => ValidationHelper.IsValueValid(command.DataType, command.Value))
            .WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
        RuleFor(x => x.DataType)
            .IsInEnum().WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
        ;
    }
}