using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.DeleteCouponCmd
{
    public class DeleteCouponHandler : IRequestHandler<DeleteCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponService _OrderCouponService;
        public DeleteCouponHandler(IOrderCouponService OrderCouponService)
        {
            _OrderCouponService = OrderCouponService;
        }
        public async Task<Result<bool>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            return await _OrderCouponService.DeleteCoupon(request.CouponID);
        }
    }
}
