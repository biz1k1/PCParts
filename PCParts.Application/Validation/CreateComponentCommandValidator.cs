using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class CreateComponentCommandValidator : AbstractValidator<CreateComponentCommand>
{
    public CreateComponentCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.SpecificationValues).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}