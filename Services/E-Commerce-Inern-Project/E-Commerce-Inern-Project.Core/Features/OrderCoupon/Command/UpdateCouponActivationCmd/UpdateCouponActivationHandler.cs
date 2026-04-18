using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponActivationCmd
{
    public class UpdateCouponActivationHandler : IRequestHandler<UpdateCouponActivationRequest, Result<bool>>
    {
        private readonly IOrderCouponService _OrderCouponService;
        public UpdateCouponActivationHandler(IOrderCouponService OrderCouponService)
        {
            _OrderCouponService = OrderCouponService;
        }
        public async Task<Result<bool>> Handle(UpdateCouponActivationRequest request, CancellationToken cancellationToken)
        {
            return await _OrderCouponService.UpdateCouponActivation(request.CouponID);
        }
    }
}
