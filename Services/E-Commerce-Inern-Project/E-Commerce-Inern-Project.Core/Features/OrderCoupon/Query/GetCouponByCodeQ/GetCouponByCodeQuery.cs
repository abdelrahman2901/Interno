using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByCodeQ
{
    public record GetCouponByCodeQuery(string Code) : IRequest<Result<OrderCouponResponse>>;
    
}
