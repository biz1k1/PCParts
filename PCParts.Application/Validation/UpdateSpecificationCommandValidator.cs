using FluentValidation;
using PCParts.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using PCParts.Application.Model.Enum;
using PCParts.Application.Command;

namespace PCParts.Application.Validation;

public class UpdateSpecificationCommandValidator : AbstractValidator<UpdateSpecificationCommand>
{
    public UpdateSpecificationCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Name) || 
                       (x.Type.HasValue && Enum.IsDefined(typeof(SpecificationDataType), x.Type)))
            .WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(x => x.Name)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        RuleFor(x => x.Type)
            .IsInEnum().WithErrorCode(ValidationErrorCode.InvalidSpecificationType);
    }
}