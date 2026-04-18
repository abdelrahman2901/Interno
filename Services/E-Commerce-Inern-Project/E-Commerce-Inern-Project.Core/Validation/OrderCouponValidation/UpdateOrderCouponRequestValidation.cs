using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
using FluentValidation;
 

namespace E_Commerce_Inern_Project.Core.Validation.OrderCouponValidation
{
    public class UpdateOrderCouponRequestValidation :AbstractValidator<UpdateCouponRequest>
    {
        public UpdateOrderCouponRequestValidation() 
        {
            RuleFor(r =>r.IsActive).NotEmpty().WithMessage("IsActive Cant Be Empty");
            //RuleFor(r =>r.IsDeleted).NotEmpty().WithMessage("IsDeleted Cant Be Empty");
            RuleFor(r =>r.Discount).NotEmpty().WithMessage("Discount Cant Be Empty");
            RuleFor(r =>r.CouponCode).NotEmpty().WithMessage("OrderCoupon Cant Be Empty");
            RuleFor(r =>r.OrderCouponID).NotEmpty().WithMessage("OrderCouponID Cant Be Empty");
        }
    }
}
