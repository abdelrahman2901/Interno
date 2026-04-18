using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IWishListRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.WishListRepo
{
    public class WishListRepository : IWishListRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WishListRepository> _logger;
        public WishListRepository(ApplicationDbContext context,ILogger<WishListRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<WishList?> CreateWishList(WishList WishList)
        {
            try
            {
                await _context.WishList.AddAsync(WishList);
                return WishList;
            } 
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile CreateWishList Exception : {message}", ex.Message);
                if(ex.InnerException != null)
                {
                _logger.LogError("Error Occured WHile CreateWishList InnerException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<WishList>> GetAllUserWishList(Guid UserID)
        {
            try
            {
                return await _context.WishList.AsNoTracking().Where(r=>r.UserID==UserID &&!r.IsDeleted).Include(p => p.Product).ThenInclude(c => c.Category).ThenInclude(pc=>pc.ParentCategory).ToListAsync();
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
                return await _context.WishList.FirstOrDefaultAsync(r=>r.WishlistID==WishListID);
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

        public async Task<WishList?> GetWishListByProdID(Guid ProductID,Guid UserID)
        {
            try
            {
                return await _context.WishList.FirstOrDefaultAsync(r => r.ProductID== ProductID&&r.UserID== UserID && !r.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile isWishListExistsByProdID Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured WHile isWishListExistsByProdID InnerException : {message}", ex.InnerException.Message);
                }
                return null ;
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
