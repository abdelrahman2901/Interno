using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IOrderCouponRepo
{
    public interface IOrderCouponRepository
    {
        public Task<IEnumerable<OrderCoupons>> GetAllCoupons();
        public Task<bool> CreateNewCoupon(OrderCoupons NewCoupon);
        public Task<OrderCoupons?> GetCoupon(Guid CouponID,bool AsTracking);
        public Task<OrderCoupons?> GetCouponByCode(string CouponCode, bool AsTracking);
        public Task<bool> SaveChanges();
    }
}
