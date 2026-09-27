using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByIDQ
{
    public record GetCouponByIDQuery(Guid CouponID) : IRequest<Result<OrderCouponResponse>>;

}
