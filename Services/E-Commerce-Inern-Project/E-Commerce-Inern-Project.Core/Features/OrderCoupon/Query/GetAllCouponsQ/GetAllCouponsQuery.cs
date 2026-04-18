using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetAllCouponsQ
{
    public record GetAllCouponsQuery() : IRequest<Result<IEnumerable<OrderCouponResponse>>>;
}
