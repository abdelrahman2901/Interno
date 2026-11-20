using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.CreateShippingCostCmd
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
