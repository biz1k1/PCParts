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
            RuleFor(x => x.componentId)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.specificationId)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.value)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
