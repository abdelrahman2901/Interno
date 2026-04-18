using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.ShippingCostValidation
{
    public class CreateShippingCostRequestValidation : AbstractValidator<CreateShippingCostRequest>
    {
        public CreateShippingCostRequestValidation()
        {
            RuleFor(x => x.ShippingCost).NotEmpty().WithMessage("ShippingCost Cant Be Empty");
            RuleFor(x => x.AraeID).NotEmpty().WithMessage("AreaID Cant Be Empty");
        }
    }
}
