using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.OrderCoupon.Command.DeleteCouponCmd
{
    public class DeleteCouponHandler : IRequestHandler<DeleteCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        

        public DeleteCouponHandler(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IInternoEventPublisher Publisher)
        {
            _orderCouponRepository = orderCouponRepository;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
        }

        public async Task<Result<bool>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon(request.CouponID, true);
            if (Coupon == null)
            {
                return Result<bool>.NotFound("Coupon Wasnt Found.");
            }
            string JsonOldValues = JsonSerializer.Serialize<OrderCoupons>(Coupon);
            Coupon.IsDeleted = true;
            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues = JsonSerializer.Serialize<OrderCoupons>(Coupon);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(OrderCoupons), JsonOldValues, JsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            return Result<bool>.Success(Coupon.IsDeleted);
        }
    }
}
