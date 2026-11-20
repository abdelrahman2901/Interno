using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ;
using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ.Models;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.ProductRepo
{
    public class ProductViewRepository : IProductViewRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductViewRepository> _logger;

        public ProductViewRepository(ApplicationDBContext context, ILogger<ProductViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<ProductRaw> FilterProductsQuery(FilterProductsDetailsQuery filter)
        {
            _logger.LogInformation("Filtering products with: {@Filter}", filter);

            IQueryable<Product> query = _context.Product
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(filter.Category) && filter.Category != "All")
            {
                query = query.Where(p => p.Category.ParentCategory.CategoryName == filter.Category);
            }

            if (!string.IsNullOrEmpty(filter.SubCategory) && filter.SubCategory != "All")
            {
                query = query.Where(p => p.Category.CategoryName == filter.SubCategory);
            }

            if (filter.Price.HasValue && filter.Price != 0)
            {
                if (filter.Price == -1)
                {
                    query = query.Where(p =>
                        ((p.SalePrice != null && p.SalePrice != 0) ? p.SalePrice : p.Price) >= 150);
                }
                else if (filter.Price == 50)
                {
                    query = query.Where(p =>
                        ((p.SalePrice != null && p.SalePrice != 0) ? p.SalePrice : p.Price) <= 50);
                }
                else if (filter.Price == 100)
                {
                    query = query.Where(p =>
                        ((p.SalePrice != null && p.SalePrice != 0) ? p.SalePrice : p.Price) <= 100);
                }
                else if (filter.Price == 150)
                {
                    query = query.Where(p =>
                        ((p.SalePrice != null && p.SalePrice != 0) ? p.SalePrice : p.Price) <= 150);
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

            query = ApplySorting(query, filter.Sort);

            return query.Select(p => new ProductRaw
            {
                ProductID = p.ProductID,
                CategoryID = p.CategoryID,
                ParentcategoryID = p.Category.ParentCategory.CategoryID,
                SizeID = p.SizeID,
                ColorID = p.ColorID,
                CategoryName = p.Category.CategoryName,
                ParentCategoryName = p.Category.ParentCategory.CategoryName,
                ProductName = p.ProductName,
                Price = p.Price,
                SalePrice = p.SalePrice,
                Stock = p.Stock,
                TotalRating = p.ProductRates.Count(r => !r.IsDeleted),
                RatingCount = p.ProductRates.Count(r => !r.IsDeleted),
                AvgRating = p.ProductRates.Where(r => !r.IsDeleted).Select(r => (double?)r.Rating).Average() ?? 0,
                SizeName = p.Size.SizeName,
                ColorName = p.Color.ColorName ?? p.Color.ColorHexCode,
                CreatedAt = p.CreatedAt,
                ProductImageUrl = p.ProductImageUrl,
                Active = p.Active,
                IsDeleted = p.IsDeleted
            });
        }

        public async Task<IEnumerable<Product>> GetAllProductsDetails()
        {
            try
            {
                return await _context.Product.AsNoTracking()
                    .Where(p => !p.IsDeleted)
                    .Include(c => c.Category).ThenInclude(c => c.ParentCategory)
                    .Include(c => c.Size)
                    .Include(c => c.Color)
                    .ToListAsync();
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
                return await _context.Product.AsNoTracking()
                    .Include(c => c.Category).ThenInclude(c => c.ParentCategory)
                    .FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted);
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

        private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy)
        {
            return sortBy switch
            {
                "priceL" => query.OrderBy(p => p.SalePrice ?? p.Price),
                "priceH" => query.OrderByDescending(p => p.SalePrice ?? p.Price),
                "nameA" => query.OrderBy(p => p.ProductName),
                "nameZ" => query.OrderByDescending(p => p.ProductName),
                "sale" => query.OrderByDescending(p => p.SalePrice),
                "rating" => query
                    .OrderByDescending(p => p.ProductRates.Where(r => !r.IsDeleted).Select(r => (double?)r.Rating).Average() ?? 0)
                    .ThenByDescending(p => p.ProductRates.Count(r => !r.IsDeleted)),
                "new" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
    }
}
