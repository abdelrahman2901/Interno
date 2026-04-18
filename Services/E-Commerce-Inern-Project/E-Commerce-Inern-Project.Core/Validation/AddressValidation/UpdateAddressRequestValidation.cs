using E_Commerce_Inern_Project.Core.Features.Address.Commands.UpdateAddressCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.AddressValidation
{
    public class UpdateAddressRequestValidation : AbstractValidator<UpdateAddressRequest>
    {
        public UpdateAddressRequestValidation() {
            RuleFor(x => x.AddressLabel)
                   .NotEmpty().WithMessage("Address label is required")
                   .MaximumLength(250).WithMessage("Address label cannot exceed 250 characters")
                   .MinimumLength(2).WithMessage("Address label must be at least 2 characters");

            RuleFor(x => x.MainAddress)
                .NotEmpty().WithMessage("Main address is required")
                .MaximumLength(500).WithMessage("Main address cannot exceed 500 characters")
                .MinimumLength(5).WithMessage("Main address must be at least 5 characters");

            RuleFor(x => x.BackUpAddress)
                .MaximumLength(500).WithMessage("Backup address cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.BackUpAddress));

            RuleFor(x => x.CityID)
                .NotEmpty().WithMessage("City is required")
                .Must(id => id != Guid.Empty).WithMessage("City ID cannot be empty");

            RuleFor(x => x.AreaID)
                .NotEmpty().WithMessage("Area is required")
                .Must(id => id != Guid.Empty).WithMessage("Area ID cannot be empty");

            RuleFor(x => x.AddressID)
                .NotEmpty().WithMessage("Address is required")
                .Must(id => id != Guid.Empty).WithMessage("AddressID cannot be empty");

            RuleFor(x => x.BackUpPhoneNumber)
                .NotEmpty().WithMessage("Backup phone number is required")
                .MaximumLength(11).WithMessage("Phone number cannot exceed 12 characters")
                .MinimumLength(10).WithMessage("Phone number must be at least 10 characters")
                .Matches(@"^\+?[0-9]{10,11}$").WithMessage("Phone number must contain only digits and optional leading +");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault flag is required");
        }
    }
}
