using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.ProductRepo
{
    public class ProductViewRepository : IProductViewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductViewRepository> _logger;
        public ProductViewRepository(ApplicationDbContext context, ILogger<ProductViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> FilterProducts(FilterProductsDetailsQuery filter)
        {
            try
            {
                _logger.LogInformation("Filtering products with: {@Filter}", filter);

                IEnumerable<Product> query = await  _context.Product.AsNoTracking()
                    .Include(p => p.Category)
                        .ThenInclude(c => c.ParentCategory) 
                    .Include(p => p.Size)
                    .Include(p => p.Color)
                    .Where(p => p.Active && !p.IsDeleted).ToListAsync();

                if (!string.IsNullOrEmpty(filter.Category) && filter.Category != "All")
                {
                    query = query.Where(p => p.Category.ParentCategory.CategoryName == filter.Category);
                }

                if (!string.IsNullOrEmpty(filter.SubCategory) && filter.SubCategory != "All")
                {
                    query = query.Where(p =>  p.Category.CategoryName == filter.SubCategory);
                } 

                if (filter.Price.HasValue && filter.Price!=0)
                {
                    if (filter.Price == -1)  
                    {
                        //query = query.Where(p => p.Price >= 150 || (p.SalePrice.HasValue && p.SalePrice >= 150));
                        query = query.Where(p => {
                            var num = (p.SalePrice != null || p.SalePrice != 0) ? p.SalePrice : p.Price;
                           return num >= 150;
                            
                            });
                    }
                    else if (filter.Price == 50)  
                    {
                        //query = query.Where(p => p.Price <= 50 || (p.SalePrice.HasValue && p.SalePrice <= 50));
                        query = query.Where(p => ((p.SalePrice != null || p.SalePrice != 0) ? p.SalePrice : p.Price) <= 50 );
                    }
                    else if (filter.Price == 100)  
                    {
                        //query = query.Where(p => p.Price <= 100 || (p.SalePrice.HasValue && p.SalePrice <= 100));
                        query = query.Where(p => ((p.SalePrice != null || p.SalePrice != 0) ? p.SalePrice : p.Price) <= 100);
                    }
                    else if (filter.Price == 150) 
                    {
                        //query = query.Where(p => p.Price <= 150 || (p.SalePrice.HasValue && p.SalePrice <= 150));
                        query = query.Where(p => ((p.SalePrice != null || p.SalePrice != 0) ? p.SalePrice : p.Price) <= 150);
                    }
                }

                if (!string.IsNullOrEmpty(filter.Size) && filter.Size != "All")
                {
                    query = query.Where(p => p.Size.SizeName == filter.Size);
                }

                if (!string.IsNullOrEmpty(filter.Color) && filter.Color != "All")
                {
                    query = query.Where(p => p.Color.ColorName == filter.Color);
                }

                if(!string.IsNullOrEmpty(filter.Sort) && filter.Sort != "All")
                {

                query = ApplySorting(query, filter.Sort);
                }

                return   query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering products Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error filtering products   InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsDetails()
        {
            try
            {
                return await _context.Product.AsNoTracking().Where(p => !p.IsDeleted).Include(c => c.Category).ThenInclude(c => c.ParentCategory).Include(c => c.Size).Include(c => c.Color).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At GetAllProductsDetails Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At GetAllProductsDetails InnerException : {Message}", ex.InnerException.Message);

                }
                return [];
            }
        }

        public async Task<Product?> GetProductDetailsByID(Guid ProductID)
        {
            try
            {

                return await _context.Product.AsNoTracking().Include(c => c.Category).ThenInclude(c => c.ParentCategory).FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured At GetProductDetailsByID Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured At GetProductDetailsByID InnerException : {Message}", ex.InnerException.Message);

                }
                return null;
            }
        }
        private IEnumerable<Product> ApplySorting(IEnumerable<Product> query, string? sortBy)
        {
            return sortBy switch
            {
                "priceL" => query.OrderBy(p => p.SalePrice ?? p.Price),
                "priceH" => query.OrderByDescending(p => p.SalePrice ?? p.Price),
                "nameA" => query.OrderBy(p => p.ProductName),
                "nameZ" => query.OrderByDescending(p => p.ProductName),
                "sale" => query.OrderByDescending(p => p.SalePrice),
                "rating" => query.OrderByDescending(p => p.Rating).ThenByDescending(p => p.TotalRating),
                "new" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt) 
            };
        }
    }
}
