using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.UserValidation.RegisterRequestValidation
{
    public class RegisterRequestValidation : AbstractValidator<RegisterUserCommand>
    {
        public RegisterRequestValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Use Valid Email Format");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
            RuleFor(x => x.ConfirmPassword).NotEmpty()
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.PhoneNumber).MinimumLength(11).WithMessage("Minimum Length for number is 11")
                .Matches("[0-9]").WithMessage("Use Valid Phone Number Format , PhoneNumber Contain only digits");
        }
    }
}
