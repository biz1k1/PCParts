using FluentValidation;
using PCParts.Application.Command;
using PCParts.Application.Helpers;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation
{
    public class CreateSpecificationValueCommandValidator:AbstractValidator<CreateSpecificationValueCommand>
    {
        public CreateSpecificationValueCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(x => x.Value)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
