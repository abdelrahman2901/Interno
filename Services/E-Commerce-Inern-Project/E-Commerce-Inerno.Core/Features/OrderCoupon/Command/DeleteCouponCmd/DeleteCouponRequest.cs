using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.DeleteCouponCmd
{
    public record DeleteCouponRequest(Guid CouponID) : IRequest<Result<bool>>;
}
