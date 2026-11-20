using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
    public class CreateOrderCouponRequestValidation : AbstractValidator<CreateCouponRequest>
    {
        public CreateOrderCouponRequestValidation()
        {
            RuleFor(r => r.CouponCode).NotEmpty().WithMessage("OrderCoupon Cant Be Empty");
            RuleFor(r => r.Discount).NotEmpty().WithMessage("Discount Cant Be Empty");
        }
    }
}
