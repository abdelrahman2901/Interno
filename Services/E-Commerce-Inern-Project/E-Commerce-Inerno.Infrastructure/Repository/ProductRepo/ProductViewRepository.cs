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

        public async Task<(IEnumerable<ProductRaw>, int total)> FilterProductsQuery(FilterProductsDetailsQuery filter, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Filtering products with: {@Filter}", filter);

            IQueryable<Product> query = _context.Product
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            if (filter.CategoryID.HasValue && filter.CategoryID != Guid.Empty)
            {
                query = query.Where(p => p.Category.ParentCategoryID == filter.CategoryID);
            }

            if (filter.SubCategoryID.HasValue && filter.SubCategoryID != Guid.Empty)
            {
                query = query.Where(p => p.CategoryID == filter.SubCategoryID);
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

            if (filter.SizeID.HasValue && filter.SizeID != Guid.Empty)
            {
                query = query.Where(p => p.SizeID == filter.SizeID);
            }

            if (filter.ColorID.HasValue && filter.ColorID != Guid.Empty)
            {
                query = query.Where(p => p.ColorID == filter.ColorID);
            }

            query = ApplySorting(query, filter.Sort);

            //var pageIDs = await query.Select(p => p.ProductID)
            //  .Skip((pageNumber - 1) * pageSize)
            //  .Take(pageSize)
            //  .ToListAsync(cancellationToken);

            var rawProducts = await query
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .Select(p => new ProductRaw
            {
                ProductID = p.ProductID,
                CategoryName = p.Category.CategoryName,
                ParentCategoryName = p.Category.ParentCategoryID != null ? p.Category.ParentCategory.CategoryName : null,
                ProductName = p.ProductName,
                Price = p.Price,
                SalePrice = p.SalePrice,
                Stock = p.Stock,
                RatingCount = p.ProductRates.Count(r => !r.IsDeleted),
                AvgRating = p.ProductRates.Where(r => !r.IsDeleted).Select(r => (double?)r.Rating).Average() ?? 0,
                SizeName = p.Size.SizeName,
                ColorName = p.Color.ColorName ?? p.Color.ColorHexCode,
                ProductImageUrl = p.ProductImageUrl,
            }).ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            return (rawProducts, total);
        }

        public async Task<IEnumerable<Product>> GetAllProductsDetails(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Product.AsNoTracking()
                    .Where(p => !p.IsDeleted)
                    .Include(c => c.Category).ThenInclude(c => c.ParentCategory)
                    .Include(c => c.Size)
                    .Include(c => c.Color)
                    .ToListAsync(cancellationToken);
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

        public async Task<Product?> GetProductDetailsByID(Guid ProductID, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Product.AsNoTracking()
                    .Include(c => c.Category).ThenInclude(c => c.ParentCategory)
                    .FirstOrDefaultAsync(p => p.ProductID == ProductID && !p.IsDeleted, cancellationToken);
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
                    .OrderByDescending(p => p.ProductRates.Count(r => !r.IsDeleted))
                    .ThenByDescending(p => p.ProductRates.Where(r => !r.IsDeleted).Select(r => (double?)r.Rating).Average() ?? 0),
                "new" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
    }
}
