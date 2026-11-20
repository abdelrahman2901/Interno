using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ApplicationDBContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                await _context.Product.AddAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while adding new Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while adding new Product InnerException : {message}", ex.InnerException.Message);
                }
                return false; 
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                return await _context.Product.AsNoTracking().Where(r => !r.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while GetAllProducts Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while GetAllProducts Product InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }

        }

        public async Task<Product?> GetProductByID(Guid? ProductID, bool AsTracking)
        {
            try
            {
                IQueryable<Product> query = AsTracking ? _context.Product : _context.Product.AsNoTracking();
                return await query.FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted);

            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while GetProductByID_Tracking Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while GetProductByID_Tracking Product InnerException : {message}", ex.InnerException.Message);
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
                _logger.LogError("error occured while SavingChanges Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while SavingChanges Product InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }

        }
    }
}
