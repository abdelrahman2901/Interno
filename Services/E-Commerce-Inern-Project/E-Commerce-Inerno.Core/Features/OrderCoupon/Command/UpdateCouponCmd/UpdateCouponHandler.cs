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

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.UpdateCouponCmd
{
    public class UpdateCouponHandler : IRequestHandler<UpdateCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        

        public UpdateCouponHandler(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon(request.OrderCouponID, true);
            if (Coupon == null)
            {
                return Result<bool>.NotFound("Coupon Wasnt Found.");
            }
            string JsonOldValues = JsonSerializer.Serialize<OrderCoupons>(Coupon);

            _mapper.Map(request, Coupon);

            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<OrderCoupons>(Coupon);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(OrderCoupons), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(Coupon != null);
        }
    }
}
