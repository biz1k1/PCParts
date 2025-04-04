using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
    }
}