using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Enums;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
   
    public record CreateCouponRequest(string CouponCode,decimal Discount, DiscountTypeEnum DiscountType,bool IsActive) : IRequest<Result<bool>>;
}