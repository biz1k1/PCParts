using FluentValidation;
using PCParts.Application.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class UpdateSpecificationValueCommandValidator : AbstractValidator<UpdateSpecificationValueCommand>
{
    public UpdateSpecificationValueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Value)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}