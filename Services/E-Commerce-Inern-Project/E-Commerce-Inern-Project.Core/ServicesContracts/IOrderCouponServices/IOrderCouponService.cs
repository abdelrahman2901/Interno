using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
 

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices
{
    public interface IOrderCouponService
    {
        public Task<Result<IEnumerable<OrderCouponResponse>>> GetAllCoupons();
        public Task<Result<OrderCouponResponse>> GetCoupon(Guid CouponID);
        public Task<Result<OrderCouponResponse>> GetCouponByCode(string CouponCode);
        public Task<Result<bool>> CreateNewCoupon(CreateCouponRequest NewCouponRequest);
        public Task<Result<bool>> UpdateCoupon(UpdateCouponRequest UpdateCoupon);
        public Task<Result<bool>> DeleteCoupon(Guid CouponID);
        public Task<Result<bool>> UpdateCouponActivation(Guid CouponID);
    }
}
