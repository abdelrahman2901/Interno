using E_Commerce_Inern_Project.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.OrderCouponRepo
{
    public class OrderCouponRepository : IOrderCouponRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<OrderCouponRepository> _logger;

        public OrderCouponRepository(ApplicationDBContext context, ILogger<OrderCouponRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateNewCoupon(OrderCoupons NewCoupon)
        {
            try
            {
                await _context.OrderCoupon.AddAsync(NewCoupon);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While CreateNewCoupon Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While CreateNewCoupon InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<IEnumerable<OrderCoupons>> GetAllCoupons()
        {
            try
            {
                return await _context.OrderCoupon.AsNoTracking().Where(r => !r.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllCoupons Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetAllCoupons InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }


        public async Task<OrderCoupons?> GetCoupon(Guid CouponID, bool AsTracking)
        {
            try
            {
                IQueryable<OrderCoupons> query = AsTracking ? _context.OrderCoupon : _context.OrderCoupon.AsNoTracking();
                return await query.FirstOrDefaultAsync(r => r.OrderCouponID == CouponID && !r.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCoupon_NoTracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetCoupon_NoTracking InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While SaveChanges Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While SaveChanges InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<OrderCoupons?> GetCouponByCode(string CouponCode, bool AsTracking)
        {
            try
            {
                IQueryable<OrderCoupons> query = AsTracking ? _context.OrderCoupon : _context.OrderCoupon.AsNoTracking();
                return await query.FirstOrDefaultAsync(r => r.CouponCode == CouponCode && !r.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCouponByCode_Tracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetCouponByCode_Tracking InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }
    }
}
