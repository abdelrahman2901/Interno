using E_Commerce_Inerno.Core.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.ViewEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace E_Commerce_Inerno.Infrastructure.Repository.OrderItemsRepo
{
    public class OrderItemsRepoistory : IOrderItemsRepoistory
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<OrderItemsRepoistory> _logger;
        public OrderItemsRepoistory(ApplicationDBContext context, ILogger<OrderItemsRepoistory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddOrderItem(OrderItems item)
        {
            try
            {
                await _context.OrderItems.AddAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> AddOrderItemList(IEnumerable<OrderItems> items)
        {
            try
            {
                await _context.OrderItems.AddRangeAsync(items);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItemsList Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItemsList InnerException : {Message}", ex.InnerException.Message);

                }
                return false;
            }
        }

        public async Task<OrderItems?> GetOrderItem(Guid OrderItemID)
        {
            try
            {
                return await _context.OrderItems.FirstOrDefaultAsync(r => r.OrderItemID == OrderItemID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public IQueryable<OrderItemDetails> GetAllOrderItemsQuery()
        {
            return _context.OrderItems
                .AsNoTracking()
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.AddedDate)
                .Select(i => new OrderItemDetails
                {
                    OrderItemID = i.OrderItemID,
                    OrderID = i.OrderID,
                    AddedDate = i.AddedDate,
                    Quantity = i.Quantity,
                    IsDeleted = i.IsDeleted,
                    Product = new ProductDetails_vw
                    {
                        ProductID = i.Product.ProductID,
                        CategoryID = i.Product.CategoryID,
                        ParentcategoryID = i.Product.Category.ParentCategory.CategoryID,
                        SizeID = i.Product.SizeID,
                        ColorID = i.Product.ColorID,
                        CategoryName = i.Product.Category.CategoryName,
                        ParentCategoryName = i.Product.Category.ParentCategory.CategoryName,
                        ProductName = i.Product.ProductName,
                        Price = i.Product.Price,
                        SalePrice = i.Product.SalePrice,
                        Stock = i.Product.Stock,
                        Rating = i.Product.ProductRates.Where(r => !r.IsDeleted).Select(r => (double?)r.Rating).Average() ?? 0,
                        TotalRating = i.Product.ProductRates.Count(r => !r.IsDeleted),
                        SizeName = i.Product.Size.SizeName,
                        ColorName = i.Product.Color.ColorName ?? i.Product.Color.ColorHexCode,
                        CreatedAt = i.Product.CreatedAt,
                        ProductImageUrl = i.Product.ProductImageUrl,
                        Active = i.Product.Active,
                        IsDeleted = i.Product.IsDeleted
                    }
                });
        }

        public async Task<bool> SaveChange()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While adding OrderItem Exception : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While adding OrderItem InnerException : {Message}", ex.InnerException.Message);

                }
                return false;
            }
        }
    }
}
