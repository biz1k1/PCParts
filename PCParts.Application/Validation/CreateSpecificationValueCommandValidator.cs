using FluentValidation;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation
{
    public class CreateSpecificationValueCommandValidator:AbstractValidator<CreateSpecificationValueCommand>
    {
        public CreateSpecificationValueCommandValidator()
        {
            RuleFor(x => x.SpecificationId)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.Value)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
