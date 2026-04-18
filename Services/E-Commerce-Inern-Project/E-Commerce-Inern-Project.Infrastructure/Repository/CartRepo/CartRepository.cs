using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.CartRepo
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(ApplicationDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart?> CreateCart(Cart Cart)
        {
            try
            {
                await _context.Cart.AddAsync(Cart);
                return Cart;
            }
            catch (Exception ex)
            {
                _logger.LogError("error Occured While Adding Cart  Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("error Occured While Adding Cart InnerException:{Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<Cart?> GetCartByUserID(Guid UserID)
        {
            try
            {
                return await _context.Cart.Include(r => r.CartItems.Where(r => !r.IsDeleted)).FirstOrDefaultAsync(x => x.UserID == UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError("error Occured While Getting getting Cart BY UserID Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error Occured While getting Cart BY UserID InnerException  :{Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<Cart?> GetUserCarItemsQuery(Guid UserID)
        {
            try
            {
                return await _context.Cart.AsNoTracking().AsSplitQuery()
                    .Include(c => c.CartItems.Where(ct => !ct.IsDeleted)).ThenInclude(ct => ct.Product).ThenInclude(s => s.Size)
                      .Include(c => c.CartItems.Where(ct => !ct.IsDeleted)).ThenInclude(ct => ct.Product).ThenInclude(s => s.Color)
                    .Include(ct => ct.CartItems.Where(ct => !ct.IsDeleted)).ThenInclude(p => p.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .FirstOrDefaultAsync(c => c.UserID == UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError("error Occured While Getting User Cart Items  Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error Occured While User Cart Items InnerException  :{Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> IsUserHasCart(Guid UserID)
        {
            try
            {
                return await _context.Cart.AnyAsync(s => s.UserID == UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError("error Occured While IsUserHasCart  Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("error Occured While IsUserHasCart InnerException:{Message}", ex.InnerException.Message);
                }
                return false;
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
                _logger.LogError("error Occured While SavingChanges  Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("error Occured While SavingChanges InnerException:{Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
