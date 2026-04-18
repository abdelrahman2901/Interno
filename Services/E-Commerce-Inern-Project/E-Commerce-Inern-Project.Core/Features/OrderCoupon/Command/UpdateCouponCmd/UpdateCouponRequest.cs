using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Enums;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd
{
    public record UpdateCouponRequest( Guid OrderCouponID ,string CouponCode ,decimal Discount, DiscountTypeEnum DiscountType, bool IsActive, bool IsDeleted) : IRequest<Result<bool>>;
}
 