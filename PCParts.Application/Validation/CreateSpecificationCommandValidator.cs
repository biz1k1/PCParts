using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class CreateSpecificationCommandValidator : AbstractValidator<CreateSpecificationCommand>
{
    public CreateSpecificationCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.Type)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .IsInEnum().WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
    }
}
