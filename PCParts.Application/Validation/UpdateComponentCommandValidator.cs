using FluentValidation;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation;

public class UpdateComponentCommandValidator : AbstractValidator<UpdateComponentCommand>
{
    public UpdateComponentCommandValidator()
    {
        RuleFor(x => x)
            .Must(command => !string.IsNullOrEmpty(command.Name) || command.CategoryId != Guid.Empty)
            .When(command => command.Id != Guid.Empty)
            .WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
    }
}