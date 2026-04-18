using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetAllCouponsQ
{
    public class GetAllCouponsHandler : IRequestHandler<GetAllCouponsQuery, Result<IEnumerable<OrderCouponResponse>>>
    {
        private readonly IOrderCouponService _OrderCouponService;
        public GetAllCouponsHandler(IOrderCouponService OrderCouponService)
        {
            _OrderCouponService = OrderCouponService;
        }
        public async Task<Result<IEnumerable<OrderCouponResponse>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
        {
            return await _OrderCouponService.GetAllCoupons();
        }
    }
}
