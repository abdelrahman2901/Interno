using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.DeleteCouponCmd
{
    public record DeleteCouponRequest(Guid CouponID) : IRequest<Result<bool>>;
}
