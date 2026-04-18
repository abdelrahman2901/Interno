using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.CartItemRepo
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartItemRepository> _logger;
        public CartItemRepository(ApplicationDbContext context, ILogger<CartItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddCartItemList(IEnumerable<CartItems> items)
        {
            try
            {
                await _context.CartItems.AddRangeAsync(items);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While CreateCartItem Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While CreateCartItem InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<CartItems?> CreateCartItem(CartItems item)
        {
            try
            {
                await _context.CartItems.AddAsync(item);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While CreateCartItem Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While CreateCartItem InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<CartItems?> GetCartItem(Guid CartItemID)
        {
            try
            {
                return await _context.CartItems.Include(r=>r.Cart).FirstOrDefaultAsync(ct=>ct.CartItemID== CartItemID && !ct.IsDeleted);
            } 
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCartItem Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetCartItem InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }
        public async Task<IEnumerable<CartItems>> GetCartItemsByCaerID(Guid CartID)
        {
            try
            {
                return await _context.CartItems.Where(r=>r.CartID==CartID&& !r.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCartItemsByCaerID Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetCartItemsByCaerID InnerException: {Message}", ex.InnerException.Message);
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
