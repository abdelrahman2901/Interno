using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponRequest, Result<bool>>
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateCouponHandler(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper)
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
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(newCoupon != null);
        }
    }
}
