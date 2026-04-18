using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product?> AddProduct(Product product)
        {
            try
            {
                await _context.Product.AddAsync(product);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while adding new Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while adding new Product InnerException : {message}", ex.InnerException.Message);
                }
                return null; 
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

        public async Task<Product?> GetProductByID_Tracking(Guid? ProductID)
        {
            try
            {
                return await _context.Product.FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted);

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
        public async Task<Product?> GetProductByID_NoTracking(Guid? ProductID)
        {
            try
            {
                return await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted);

            }
            catch (Exception ex)
            {
                _logger.LogError("error occured while GetProductByID_NoTracking Product Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("error occured while GetProductByID_NoTracking Product InnerException : {message}", ex.InnerException.Message);
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
