using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.OrderCouponServices
{
    public class OrderCouponService : IOrderCouponService
    {
        private readonly IOrderCouponRepository _orderCouponRepository;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        private readonly Guid AdminID= Guid.Empty;
        public OrderCouponService(IOrderCouponRepository orderCouponRepository, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _orderCouponRepository = orderCouponRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            this.AdminID = UserRepo.GetApplicationUserByEmail("Admin@gmail.com").Result.Id;
        }

        public async Task<Result<bool>> CreateNewCoupon(CreateCouponRequest NewCouponRequest)
        {
            OrderCoupons? newCoupon = await _orderCouponRepository.CreateNewCoupon(_mapper.Map<OrderCoupons>(NewCouponRequest));
            if (newCoupon == null)
            {

                return Result<bool>.InternalError("Failed To Create Coupon");
            }
            if(!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues=JsonSerializer.Serialize<OrderCoupons>(newCoupon);
            AuditRequest AuditRequest = new(this.AdminID, ActionTypeEnum.Create, nameof(OrderCoupons), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(newCoupon!=null);
        }

        public async Task<Result<bool>> DeleteCoupon(Guid CouponID)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon_Tracking(CouponID);
            if (Coupon == null)
            {
                return Result<bool>.NotFound("Coupon Wasnt Found.");
            }
            string JsonOldValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);
            Coupon.IsDeleted = true;
            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);
            AuditRequest AuditRequest = new(this.AdminID, ActionTypeEnum.Delete, nameof(OrderCoupons), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(Coupon.IsDeleted) ;
        }
        public async Task<Result<bool>> UpdateCouponActivation(Guid CouponID)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon_Tracking(CouponID);
            if (Coupon == null)
            {
                return Result<bool>.NotFound("Coupon Wasnt Found.");
            }
            string JsonOldValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);

            Coupon.IsActive = !Coupon.IsActive;
            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);
            AuditRequest AuditRequest = new(this.AdminID, ActionTypeEnum.Update, nameof(OrderCoupons),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(Coupon.IsDeleted) ;
        }

        public async Task<Result<IEnumerable<OrderCouponResponse>>> GetAllCoupons()
        {
            IEnumerable<OrderCoupons> Coupons = await _orderCouponRepository.GetAllCoupons();
            if (!Coupons.Any())
            {
                return Result<IEnumerable<OrderCouponResponse>>.NotFound("Coupons Wasnt Found.");
            }
            return Result<IEnumerable<OrderCouponResponse>>.Success(Coupons.Select(s=>_mapper.Map<OrderCouponResponse>(s)));
        }

        public async Task<Result<bool>> UpdateCoupon(UpdateCouponRequest UpdateCoupon)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon_Tracking(UpdateCoupon.OrderCouponID);
            if (Coupon == null)
            {
                return Result<bool>.NotFound("Coupon Wasnt Found.");
            }
            string JsonOldValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);

            _mapper.Map(UpdateCoupon,Coupon);

            if (!await _orderCouponRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaevChanges");
            }

            string JsonNewValues=JsonSerializer.Serialize<OrderCoupons>(Coupon);
            AuditRequest AuditRequest = new(this.AdminID, ActionTypeEnum.Update, nameof(OrderCoupons),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            return Result<bool>.Success(Coupon!=null);
        }

        public async Task<Result<OrderCouponResponse>> GetCoupon(Guid CouponID)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCoupon_NoTracking(CouponID);
            if (Coupon == null)
            {
                return Result<OrderCouponResponse>.NotFound("Coupon Wasnt Found.");
            }
            return  Result<OrderCouponResponse>.Success(_mapper.Map<OrderCouponResponse>(Coupon));
        }
        public async Task<Result<OrderCouponResponse>> GetCouponByCode(string CouponCode)
        {
            OrderCoupons? Coupon = await _orderCouponRepository.GetCouponByCode_NoTracking(CouponCode);
            if (Coupon == null)
            {
                return Result<OrderCouponResponse>.NotFound("Coupon Wasnt Found.");
            }
            return  Result<OrderCouponResponse>.Success(_mapper.Map<OrderCouponResponse>(Coupon));
        }
    }
}
