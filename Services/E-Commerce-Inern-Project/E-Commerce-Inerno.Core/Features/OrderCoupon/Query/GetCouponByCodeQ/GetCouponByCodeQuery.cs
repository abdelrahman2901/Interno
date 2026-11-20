using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Query.GetCouponByCodeQ
{
    public record GetCouponByCodeQuery(string Code) : IRequest<Result<OrderCouponResponse>>;
    
}
