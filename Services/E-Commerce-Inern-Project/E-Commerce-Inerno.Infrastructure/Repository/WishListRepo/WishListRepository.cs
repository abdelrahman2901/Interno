using E_Commerce_Inerno.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.WishListRepo
{
    public class WishListRepository : IWishListRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<WishListRepository> _logger;
        public WishListRepository(ApplicationDBContext context, ILogger<WishListRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateWishList(WishList WishList)
        {
            try
            {
                await _context.WishList.AddAsync(WishList);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile CreateWishList Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured WHile CreateWishList InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<IEnumerable<WishList>> GetAllUserWishList(Guid UserID)
        {
            try
            {
                return await _context.WishList.AsNoTracking().Where(r => r.UserID == UserID && !r.IsDeleted).Include(p => p.Product).ThenInclude(c => c.Category).ThenInclude(pc => pc.ParentCategory).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile GetAllUserWishList Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured WHile GetAllUserWishList InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<WishList?> GetWishList(Guid WishListID)
        {
            try
            {
                return await _context.WishList.FirstOrDefaultAsync(r => r.WishlistID == WishListID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile GetWishList Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured WHile GetWishList InnerException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<WishList?> GetWishListByProdID(Guid ProductID, Guid UserID)
        {
            try
            {
                return await _context.WishList.FirstOrDefaultAsync(r => r.ProductID == ProductID && r.UserID == UserID && !r.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile isWishListExistsByProdID Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured WHile isWishListExistsByProdID InnerException : {message}", ex.InnerException.Message);
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
                _logger.LogError("Error Occured WHile SaveChanges  Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured  WHile SaveChanges InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
