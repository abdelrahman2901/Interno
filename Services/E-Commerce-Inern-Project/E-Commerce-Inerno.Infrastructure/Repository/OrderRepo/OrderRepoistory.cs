using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.ViewEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.OrderRepo
{
    public class OrderRepoistory : IOrdersRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<OrderRepoistory> _logger;
        public OrderRepoistory(ApplicationDBContext context, ILogger<OrderRepoistory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateOrder(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While SaveChanges Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While SaveChanges InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return await _context.Orders.AsNoTracking()
                     .Include(r => r.User)
                    .Include(r => r.Payment)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .Include(r => r.Address)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllOrders Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetAllOrders InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public IQueryable<OrderDetails> GetAllOrdersQuery()
        {
            return _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderDetails
                {
                    OrderID = o.OrderID,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    Subtotal = o.Subtotal,
                    DiscountAmount = o.DiscountAmount,
                    TotalAmount = o.TotalAmount,
                    OrderStatus = o.OrderStatus,
                    UserName = o.User.PersonName,
                    UserEmail = o.User.Email,
                    PaymentMethod = o.Payment.PaymentMethod.ToString(),
                    Address = new AddressResponse
                    {
                        AddressID = o.Address.AddressID,
                        UserID = o.Address.UserID,
                        CityID = o.Address.CityID,
                        AreaID = o.Address.AreaID,
                        AddressLabel = o.Address.AddressLabel,
                        MainAddress = o.Address.MainAddress,
                        BackUpAddress = o.Address.BackUpAddress,
                        BackUpPhoneNumber = o.Address.BackUpPhoneNumber,
                        IsDeleted = o.Address.IsDeleted,
                        IsDefault = o.Address.IsDefault
                    },
                    ShippingCosts = o.ShippingCosts == null ? null! : new ShippingCostDetails
                    {
                        ShippingCostID = o.ShippingCosts.ShippingCostID,
                        ShippingCost = o.ShippingCosts.ShippingCost,
                        IsDeleted = o.ShippingCosts.IsDeleted,
                        Area = new AreaResponse
                        {
                            AreaID = o.ShippingCosts.Area.AreaID,
                            CityID = o.ShippingCosts.Area.CityID,
                            AreaName = o.ShippingCosts.Area.AreaName
                        }
                    },
                    OrderCoupon = o.OrderCoupon == null ? null : new OrderCouponResponse
                    {
                        OrderCouponID = o.OrderCoupon.OrderCouponID,
                        CouponCode = o.OrderCoupon.CouponCode,
                        Discount = o.OrderCoupon.Discount,
                        DiscountType = o.OrderCoupon.DiscountType,
                        IsActive = o.OrderCoupon.IsActive,
                        IsDeleted = o.OrderCoupon.IsDeleted
                    },
                    OrderItems = o.OrderItems
                        .Where(i => !i.IsDeleted)
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
                        })
                });
        }

        public async Task<IEnumerable<Order>> GetAllUserOrders(Guid UserID)
        {
            try
            {
                return await _context.Orders.AsNoTracking().Where(r => r.UserID == UserID)
                    .Include(r => r.ShippingCosts)
                    .Include(r => r.OrderCoupon)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .Include(r => r.Address).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllUserOrders Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetAllUserOrders InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }


        public async Task<Order?> GetOrderByID(Guid OrderID, bool AsTracking)
        {
            try
            {

                IQueryable<Order> query = AsTracking ? _context.Orders : _context.Orders.AsNoTracking();
                return await query
                     .Include(r => r.Payment)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Category).ThenInclude(c => c.ParentCategory)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Size)
                    .Include(r => r.OrderItems).ThenInclude(r => r.Product).ThenInclude(p => p.Color)
                    .Include(r => r.Address)
                    .Include(r => r.ShippingCosts)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.OrderID == OrderID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetOrderByID_NoTracking Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetOrderByID_NoTracking InnerException: {Message}", ex.InnerException.Message);
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
                _logger.LogError("Error Occured While SaveChanges Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While SaveChanges InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
