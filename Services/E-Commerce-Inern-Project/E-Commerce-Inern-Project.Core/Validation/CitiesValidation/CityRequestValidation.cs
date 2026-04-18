using E_Commerce_Inern_Project.Core.Features.City.Commands.CreateCityCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.CitiesValidation
{
    public class CityRequestValidation : AbstractValidator<CityRequest>
    {
        public CityRequestValidation() {
            RuleFor(x => x.CityName)
                    .NotEmpty().WithMessage("City name is required")
                    .MaximumLength(200).WithMessage("City name cannot exceed 200 characters")
                    .MinimumLength(2).WithMessage("City name must be at least 2 characters")
                    .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("City name can only contain letters, spaces, hyphens, and apostrophes");
        }
    }
}
