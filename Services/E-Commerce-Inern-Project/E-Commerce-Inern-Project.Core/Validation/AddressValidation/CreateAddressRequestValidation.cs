using E_Commerce_Inern_Project.Core.Features.Address.Commands.CreateAddressCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.AddressValidation
{
    public class CreateAddressRequestValidation:AbstractValidator<CreateAddressRequest>
    {
        public CreateAddressRequestValidation() {

            RuleFor(x => x.AddressLabel)
                .NotEmpty().WithMessage("Address label is required")
                .MaximumLength(250).WithMessage("Address label cannot exceed 250 characters")
                .MinimumLength(2).WithMessage("Address label must be at least 2 characters");

            RuleFor(x => x.MainAddress)
                .NotEmpty().WithMessage("Main address is required")
                .MaximumLength(500).WithMessage("Main address cannot exceed 500 characters")
                .MinimumLength(5).WithMessage("Main address must be at least 5 characters");

            

            RuleFor(x => x.CityID)
                .NotEmpty().WithMessage("City is required")
                .Must(id => id != Guid.Empty).WithMessage("City ID cannot be empty");

            RuleFor(x => x.AreaID)
                .NotEmpty().WithMessage("Area is required")
                .Must(id => id != Guid.Empty).WithMessage("Area ID cannot be empty");

            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage("User is required")
                .Must(id => id != Guid.Empty).WithMessage("UserID   cannot be empty");

            

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault flag is required");
        }
    }
}
