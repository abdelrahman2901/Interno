using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponService _OrderCouponService;
        public CreateCouponHandler(IOrderCouponService OrderCouponService)
        {
            _OrderCouponService = OrderCouponService;
        }
        public async Task<Result<bool>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            return await _OrderCouponService.CreateNewCoupon(request);
        }
    }
}
