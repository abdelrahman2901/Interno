using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Query.GetCouponByCodeQ
{
    public class GetCouponByCodeHandler : IRequestHandler<GetCouponByCodeQuery, Result<OrderCouponResponse>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;

        public GetCouponByCodeHandler(IOrderCouponRepository orderCouponRepository, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderCouponResponse>> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCouponByCode(request.Code, false);
            if (Coupon == null)
            {
                return Result<OrderCouponResponse>.NotFound("Coupon Wasnt Found.");
            }
            return Result<OrderCouponResponse>.Success(_mapper.Map<OrderCouponResponse>(Coupon));
        }
    }
}
