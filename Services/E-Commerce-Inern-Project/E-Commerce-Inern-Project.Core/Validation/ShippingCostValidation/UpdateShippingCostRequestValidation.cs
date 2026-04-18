using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd;
using FluentValidation;
 

namespace E_Commerce_Inern_Project.Core.Validation.ShippingCostValidation
{
    public class UpdateShippingCostRequestValidation:AbstractValidator<UpdateShippingCostRequest>
    {
        public UpdateShippingCostRequestValidation()
        {
            RuleFor(r=>r.ShippingCost).NotEmpty().WithMessage("ShippingCost Cant Be Empty");
            RuleFor(r=>r.ShippingCostID).NotEmpty().WithMessage("ShippingCostID Cant Be Empty");
            RuleFor(r=>r.AreaID).NotEmpty().WithMessage("AraeID Cant Be Empty");
            //RuleFor(r=>r.IsDeleted).NotEmpty().WithMessage("IsDeleted Cant Be Empty");
        }
    }
}
