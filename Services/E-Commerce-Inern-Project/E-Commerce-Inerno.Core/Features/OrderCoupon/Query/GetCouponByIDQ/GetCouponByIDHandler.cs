using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Query.GetCouponByIDQ
{
    public class GetCouponByIDHandler : IRequestHandler<GetCouponByIDQuery, Result<OrderCouponResponse>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;

        public GetCouponByIDHandler(IOrderCouponRepository orderCouponRepository, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderCouponResponse>> Handle(GetCouponByIDQuery request, CancellationToken cancellationToken)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon(request.CouponID, false);
            if (Coupon == null)
            {
                return Result<OrderCouponResponse>.NotFound("Coupon Wasnt Found.");
            }
            return Result<OrderCouponResponse>.Success(_mapper.Map<OrderCouponResponse>(Coupon));
        }
    }
}
