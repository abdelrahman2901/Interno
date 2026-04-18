using E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.CitiesValidation
{
    public class CityUpdateRequestValidation:AbstractValidator<CityUpdateRequest>
    {
        public CityUpdateRequestValidation() {
            RuleFor(x => x.CityID)
                    .NotEmpty().WithMessage("City ID is required")
                    .Must(id => id != Guid.Empty).WithMessage("City ID cannot be empty");

            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("City name is required")
                .MaximumLength(200).WithMessage("City name cannot exceed 200 characters")
                .MinimumLength(2).WithMessage("City name must be at least 2 characters")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("City name can only contain letters, spaces, hyphens, and apostrophes");

        }
    }
}
