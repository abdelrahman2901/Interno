using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByCodeQ
{
    public class GetCouponByCodeHandler : IRequestHandler<GetCouponByCodeQuery, Result<OrderCouponResponse>>
    {
        private readonly IOrderCouponService _orderCouponService;
        public GetCouponByCodeHandler(IOrderCouponService orderCouponService)
        {
            _orderCouponService = orderCouponService;
        }

        public async Task<Result<OrderCouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
        {
            return await _orderCouponService.GetCouponByCode(request.Code);
        }
    }
}
