using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class UpdateSpecificationCommandValidator : AbstractValidator<UpdateSpecificationCommand>
{
    public UpdateSpecificationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.Type)
            .IsInEnum().WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
    }
}