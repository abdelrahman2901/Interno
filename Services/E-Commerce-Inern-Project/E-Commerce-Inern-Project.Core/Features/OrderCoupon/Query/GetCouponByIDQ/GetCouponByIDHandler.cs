using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByIDQ
{
    public class GetCouponByIDHandler : IRequestHandler<GetCouponByIDQuery, Result<OrderCouponResponse>>
    {
        private readonly IOrderCouponService _CouponService;
        public GetCouponByIDHandler(IOrderCouponService couponService)
        {
            _CouponService = couponService;
        }

        public async Task<Result<OrderCouponResponse>> Handle(GetCouponByIDQuery request, CancellationToken cancellationToken)
        {
            return await _CouponService.GetCoupon(request.CouponID);
        }
    }
}
