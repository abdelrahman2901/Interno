using E_Commerce_Inerno.Core.Features.Auth.Commands.LoginUser;
using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.LoginUser
{
    public class LoginRequestValidation : AbstractValidator<LoginUserCommand>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(5).WithMessage("Password must be at least 5 characters long");
        }
    }
}
