using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.OrderRepo
{
    public class OrderRepoistory : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepoistory> _logger;
        public OrderRepoistory(ApplicationDbContext context, ILogger<OrderRepoistory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Order?> CreateOrder(Order order)
        {
            try
            {
                 await _context.Orders.AddAsync(order);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While SaveChanges Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While SaveChanges InnerException: {Message}", ex.InnerException.Message);
                }
                return null; 
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return await _context.Orders.AsNoTracking()
                     .Include(r => r.User)
                    .Include(r => r.Payment)
                    .Include(r=>r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .Include(r=>r.Address)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllOrders Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetAllOrders InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<IEnumerable<Order>> GetAllUserOrders(Guid UserID)
        {
            try
            {
                return await _context.Orders.AsNoTracking().Where(r=>r.UserID== UserID)
                    .Include(r => r.ShippingCosts)
                    .Include(r=>r.OrderCoupon) 
                    .Include(r => r.OrderItems).ThenInclude(r=>r.Product).ThenInclude(p=>p.Category).ThenInclude(c=>c.ParentCategory)
                    .Include(r => r.Address).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllUserOrders Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetAllUserOrders InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<Order?> GetOrderByID_Tracking(Guid OrderID)
        {
            try
            {
                return await _context.Orders.FirstOrDefaultAsync(r=>r.OrderID==OrderID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetOrderByID_Tracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetOrderByID_Tracking InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }
        public async Task<Order?> GetOrderByID_NoTracking(Guid OrderID)
        {
            try
            {
                return await _context.Orders.AsNoTracking()
                     .Include(r => r.Payment)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Size)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Color)
                    .Include(r => r.Address)
                    .Include(r => r.ShippingCosts)
                    .Include(r=>r.User)
                    .FirstOrDefaultAsync(r=>r.OrderID==OrderID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetOrderByID_NoTracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetOrderByID_NoTracking InnerException: {Message}", ex.InnerException.Message);
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
    }
}
