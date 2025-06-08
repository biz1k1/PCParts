using FluentValidation;
using PCParts.Application.Command;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Validation
{
    public class CreatePendingUserCommandValidator:AbstractValidator<CreatePendingUserCommand>
    {
        public CreatePendingUserCommandValidator()
        {
            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3).WithErrorCode(ValidationErrorCode.TooSmall)
                .MaximumLength(15).WithErrorCode(ValidationErrorCode.TooLong);
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(6).WithErrorCode(ValidationErrorCode.TooSmall)
                .MaximumLength(30).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}
