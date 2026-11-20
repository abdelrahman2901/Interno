using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
   
    public record CreateCouponRequest(string CouponCode,decimal Discount, DiscountTypeEnum DiscountType,bool IsActive) : IRequest<Result<bool>>;
}