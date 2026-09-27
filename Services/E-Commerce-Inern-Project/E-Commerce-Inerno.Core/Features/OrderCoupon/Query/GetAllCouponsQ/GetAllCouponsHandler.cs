using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetAllCouponsQ
{
    public class GetAllCouponsHandler : IRequestHandler<GetAllCouponsQuery, Result<IEnumerable<OrderCouponResponse>>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;

        public GetAllCouponsHandler(IOrderCouponRepository orderCouponRepository, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<OrderCouponResponse>>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OrderCoupons> Coupons = await _orderCouponRepository.GetAllCoupons();
            if (!Coupons.Any())
            {
                return Result<IEnumerable<OrderCouponResponse>>.NotFound("Coupons Wasnt Found.");
            }
            return Result<IEnumerable<OrderCouponResponse>>.Success(Coupons.Select(s => _mapper.Map<OrderCouponResponse>(s)));
        }
    }
}
