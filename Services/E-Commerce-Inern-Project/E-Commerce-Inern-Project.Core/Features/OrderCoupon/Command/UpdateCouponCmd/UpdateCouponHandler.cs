using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd
{
    public class UpdateCouponHandler : IRequestHandler<UpdateCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponService _OrderCouponService;
        public UpdateCouponHandler(IOrderCouponService OrderCouponService)
        {
            _OrderCouponService = OrderCouponService;
        }
        public async Task<Result<bool>> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            return await _OrderCouponService.UpdateCoupon(request);
        }
    }
}
