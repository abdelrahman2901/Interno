using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace E_Commerce_Inern_Project.Infrastructure.Repository.OrderItemsRepo
{
    public class OrderItemsRepoistory : IOrderItemsRepoistory
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderItemsRepoistory> _logger;
        public OrderItemsRepoistory(ApplicationDbContext context, ILogger<OrderItemsRepoistory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddOrderItem(OrderItems item)
        {
            try
            {
                await _context.OrderItems.AddAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> AddOrderItemList(IEnumerable<OrderItems> items)
        {
            try
            {
                await _context.OrderItems.AddRangeAsync(items);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItemsList Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItemsList InnerException : {Message}", ex.InnerException.Message);

                }
                return false;
            }
        }

        public async Task<OrderItems?> GetOrderItem(Guid OrderItemID)
        {
            try
            {
                return await _context.OrderItems.FirstOrDefaultAsync(r => r.OrderItemID == OrderItemID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChange()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);

                }
                return false;
            }
        }
    }
}
