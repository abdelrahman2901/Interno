using E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.AreasValidation
{
    public class AreaRequestValidation :AbstractValidator<AreaRequest>
    {
        public AreaRequestValidation() {
            RuleFor(x => x.CityID)
                 .NotEmpty().WithMessage("City is required")
                 .Must(id => id != Guid.Empty).WithMessage("City ID cannot be empty");

            RuleFor(x => x.AreaName)
                .NotEmpty().WithMessage("Area name is required")
                .MaximumLength(200).WithMessage("Area name cannot exceed 200 characters")
                .MinimumLength(2).WithMessage("Area name must be at least 2 characters")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Area name can only contain letters, spaces, hyphens, and apostrophes");

        }
    }
}
