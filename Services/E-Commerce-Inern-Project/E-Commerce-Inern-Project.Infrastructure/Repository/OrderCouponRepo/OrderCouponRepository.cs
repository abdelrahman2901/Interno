using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.OrderCouponRepo
{
    public class OrderCouponRepository : IOrderCouponRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderCouponRepository> _logger;

        public OrderCouponRepository(ApplicationDbContext context, ILogger<OrderCouponRepository>logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OrderCoupons?> CreateNewCoupon(OrderCoupons NewCoupon)
        {
            try
            {
                await _context.OrderCoupon.AddAsync(NewCoupon);
                return NewCoupon;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While CreateNewCoupon Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While CreateNewCoupon InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<OrderCoupons>> GetAllCoupons()
        {
            try
            {
                return await _context.OrderCoupon.AsNoTracking().Where(r=>!r.IsDeleted).ToListAsync();
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

        public async Task<OrderCoupons?> GetCoupon_Tracking(Guid CouponID)
        {
            try
            {
                return await _context.OrderCoupon.FirstOrDefaultAsync(r => r.OrderCouponID == CouponID  && !r.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCoupon_Tracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetCoupon_Tracking InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<OrderCoupons?> GetCoupon_NoTracking(Guid CouponID)
        {
            try
            {
                return await _context.OrderCoupon.AsNoTracking().FirstOrDefaultAsync(r => r.OrderCouponID == CouponID  && !r.IsDeleted);
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

        public async Task<OrderCoupons?> GetCouponByCode_NoTracking(string CouponCode)
        {
            try
            {
                return await _context.OrderCoupon.AsNoTracking().FirstOrDefaultAsync(r => r.CouponCode == CouponCode && !r.IsDeleted);
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
