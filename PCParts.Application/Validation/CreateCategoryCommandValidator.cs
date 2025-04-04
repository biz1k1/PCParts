using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}