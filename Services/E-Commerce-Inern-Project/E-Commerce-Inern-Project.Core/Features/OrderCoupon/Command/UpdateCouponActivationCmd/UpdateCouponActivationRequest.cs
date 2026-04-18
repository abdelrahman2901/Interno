using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponActivationCmd
{
    public record UpdateCouponActivationRequest(Guid CouponID) : IRequest<Result<bool>>;
    
}
