using E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword;
using FluentValidation;
 

namespace E_Commerce_Inern_Project.Core.Validation.UserValidation.ChangePasswordValidation
{
    public class ChangePasswordValidation :AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangePasswordValidation()
        {
            RuleFor(x => x.UserID).NotEmpty().WithMessage("UserID is required");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required")
                .MinimumLength(5).WithMessage("Current password must be at least 5 characters long");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(5).WithMessage("New password must be at least 5 characters long");
            RuleFor(x => x.confirmPassword).NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.NewPassword).WithMessage("Confirm password must match the new password");
        }
    }
}
