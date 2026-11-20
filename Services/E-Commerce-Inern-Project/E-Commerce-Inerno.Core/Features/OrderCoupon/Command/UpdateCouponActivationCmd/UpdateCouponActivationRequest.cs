using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.UpdateCouponActivationCmd
{
    public record UpdateCouponActivationRequest(Guid CouponID) : IRequest<Result<bool>>;
    
}
