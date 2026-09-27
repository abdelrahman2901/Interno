using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.DeleteCouponCmd
{
    public class DeleteCouponHandler : IRequestHandler<DeleteCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteCouponHandler(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
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
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(Coupon.IsDeleted);
        }
    }
}
