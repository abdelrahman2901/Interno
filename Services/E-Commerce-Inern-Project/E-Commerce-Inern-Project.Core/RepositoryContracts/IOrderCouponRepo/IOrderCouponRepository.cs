using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderCouponRepo
{
    public interface IOrderCouponRepository
    {
        public Task<IEnumerable<OrderCoupons>> GetAllCoupons();
        public Task<OrderCoupons?> CreateNewCoupon(OrderCoupons NewCoupon);
        public Task<OrderCoupons?> GetCoupon_NoTracking(Guid CouponID);
        public Task<OrderCoupons?> GetCouponByCode_NoTracking(string CouponCode);
        public Task<OrderCoupons?> GetCoupon_Tracking(Guid CouponID);
        public Task<bool> SaveChanges();
    }
}
