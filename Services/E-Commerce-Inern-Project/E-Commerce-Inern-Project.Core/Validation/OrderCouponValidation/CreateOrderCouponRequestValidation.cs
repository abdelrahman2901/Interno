
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Validation.OrderCouponValidation
{
    public class CreateOrderCouponRequestValidation :AbstractValidator<CreateCouponRequest>
    {
        public CreateOrderCouponRequestValidation() 
        {
            RuleFor(r => r.CouponCode).NotEmpty().WithMessage("OrderCoupon Cant Be Empty");
            RuleFor(r => r.Discount).NotEmpty().WithMessage("Discount Cant Be Empty");
        }
    }
}
