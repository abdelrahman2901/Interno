using AutoMapper;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        

        public CreateCouponHandler(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            var newCoupon = _mapper.Map<OrderCoupons>(request);
            bool result = await _orderCouponRepository.CreateNewCoupon(newCoupon);
            if (!result)
            {
                return Result<bool>.InternalError("Failed To Create Coupon");
            }
            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<OrderCoupons>(newCoupon);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Create, nameof(OrderCoupons), null, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(newCoupon != null);
        }
    }
}
