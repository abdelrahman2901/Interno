using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inern_Project.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.ProductRatesRepo
{
    public class ProductRatesRepository : IProductRatesRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductRatesRepository> _logger;
        public ProductRatesRepository(ApplicationDBContext context, ILogger<ProductRatesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddListAsync(IEnumerable<ProductRates> Rates)
        {
            try
            {
                await _context.ProductRates.AddRangeAsync(Rates);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In AddAsync Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In AddAsync InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<IEnumerable<ProductRates>> GetAllProductRates()
        {
            try
            {
                return await _context.ProductRates.AsNoTracking().Where(pr => !pr.IsDeleted).Include(p => p.Product).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetAllProductRates Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetAllProductRates InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<IEnumerable<ProductRates>> GetAllProductRatesForProduct(Guid ProductID)
        {
            try
            {
                return await _context.ProductRates.AsNoTracking().Where(pr => pr.ProductID == ProductID && !pr.IsDeleted).Include(p => p.Product).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetAllProductRatesForProduct Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetAllProductRatesForProduct InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

  

        public async Task<ProductRates?> GetProductRateByID(Guid id,bool AsTracking)
        {
            try
            {
                IQueryable<ProductRates> query = AsTracking ? _context.ProductRates : _context.ProductRates.AsNoTracking();
                return await query.Include(p => p.Product).FirstOrDefaultAsync(pr => pr.RateID == id && !pr.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetProductRateByID Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetProductRateByID InnerException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<ProductRates>> GetUserRating(Guid UserID)
        {
            try
            {
                return await _context.ProductRates.AsNoTracking().Where(pr => pr.UserID==UserID &&!pr.IsDeleted).Include(p => p.Product).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetAllProductRates Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetAllProductRates InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<bool> isUserHasRatingForProduct(ProductRateRequest request)
        {

            try
            {
                return await _context .ProductRates.AnyAsync(r=>r.ProductID==request.ProductID&&r.UserID==request.UserID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In isUserHasRatingForProduct Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In isUserHasRatingForProduct InnerException : {message}", ex.InnerException.Message);
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
                _logger.LogError("Error Occured In SaveChanges Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In SaveChanges InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
