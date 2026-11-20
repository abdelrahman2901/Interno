using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.UpdateShippingCostCmd
{
    public class UpdateShippingCostRequestValidation : AbstractValidator<UpdateShippingCostRequest>
    {
        public UpdateShippingCostRequestValidation()
        {
            RuleFor(r => r.ShippingCost).NotEmpty().WithMessage("ShippingCost Cant Be Empty");
            RuleFor(r => r.ShippingCostID).NotEmpty().WithMessage("ShippingCostID Cant Be Empty");
            RuleFor(r => r.AreaID).NotEmpty().WithMessage("AraeID Cant Be Empty");
        }
    }
}
