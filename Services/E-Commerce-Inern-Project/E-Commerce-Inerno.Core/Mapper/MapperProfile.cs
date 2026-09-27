using AutoMapper;
using E_Commerce_Inern_Project.Core.Features.Addresses.Commands.CreateAddressCmd;
using E_Commerce_Inern_Project.Core.Features.Areas.Commands.CreateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Areas.Commands.UpdateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.UpdateUser;
using E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.CreateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.BannerSlides.Command.UpdateBannerCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd;
using E_Commerce_Inern_Project.Core.Features.Categories.Commands.CreateCategoryCommad;
using E_Commerce_Inern_Project.Core.Features.Cities.Commands.CreateCityCmd;
using E_Commerce_Inern_Project.Core.Features.Cities.Commands.UpdateCityCmd;
using E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Payment.Command.CreateNewPaymentCmd;
using E_Commerce_Inern_Project.Core.Features.Products.Commands.CreateProductCommand;
using E_Commerce_Inern_Project.Core.Features.ShippingCost.Command.CreateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCost.Command.UpdateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand;
using E_Commerce_Inern_Project.Core.Features.WishLists.Commands.CreateWishListCmd;
using E_Commerce_Inerno.Core.Features.Addresses.Commands.UpdateAddressCmd;
using E_Commerce_Inerno.Core.Features.Categories.Commands.UpdateCategoryCommand;
using E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductCommand;
using E_Commerce_Interno.Domain.DTO.AddressDTO;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using E_Commerce_Interno.Domain.DTO.CartDTO;
using E_Commerce_Interno.Domain.DTO.CartItemDTO;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using E_Commerce_Interno.Domain.DTO.SizeDTO;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.ViewEntites;
namespace E_Commerce_Inern_Project.Core.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //User
            CreateMap<RegisterUserCommand, ApplicationUser>().ForMember(dest => dest.UserID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser, AuthUserDetailsDTO>()
                .ForMember(dest => dest.Roles, src => src.MapFrom(s => s.Roles.Select(r => r.Role.RoleEnumID)))
                .ForMember(dest => dest.userID, src => src.MapFrom(s => s.UserID));
            CreateMap<UpdateUserCommand, ApplicationUser>();

            //Product
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<ProductUpdateRequest, Product>();
            CreateMap<Product, ProductDetails_vw>()
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(s => s.Category.CategoryName))
                .ForMember(dest => dest.ParentCategoryName, src => src.MapFrom(s => s.Category.ParentCategory.CategoryName))
                .ForMember(dest => dest.ParentcategoryID, src => src.MapFrom(s => s.Category.ParentCategory.CategoryID))
                .ForMember(dest => dest.SizeName, src => src.MapFrom(s => s.Size.SizeName))
                .ForMember(dest => dest.ColorName, src => src.MapFrom(s => s.Color.ColorName ?? s.Color.ColorHexCode));

            //Category
            CreateMap<Category, CategoryResponse>();
            CreateMap<Category, SubCategoryResponse>();
            CreateMap<CategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            //colors
            CreateMap<Colors, ColorResponse>();
            CreateMap<ColorRequest, Colors>().ForMember(dest => dest.ColorID, opt => opt.MapFrom(src => Guid.NewGuid()));

            //size
            CreateMap<Size, SizeResponse>();
            CreateMap<SizeRequest, Size>().ForMember(dest => dest.SizeID, opt => opt.MapFrom(src => Guid.NewGuid()));

            //area
            CreateMap<Area, AreaResponse>();
            CreateMap<AreaRequest, Area>().ForMember(dest => dest.AreaID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<AreaUpdateRequest, Area>();
            //city
            CreateMap<City, CityResponse>();
            CreateMap<CityRequest, City>().ForMember(dest => dest.CityID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<CityUpdateRequest, City>();


            //Address
            CreateMap<Address, AddressResponse>();
            CreateMap<CreateAddressRequest, Address>().ForMember(dest => dest.AddressID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<UpdateAddressRequest, Address>();
            CreateMap<Address, AddressDetails>().ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.AreaName));


            //Cart
            CreateMap<Cart, CartDetailsResponse>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
            CreateMap<Cart, CartResponse>();

            //CartItem
            CreateMap<CartItems, CartItemResponse>();
            CreateMap<AddCartItemListRequest, CartItems>()
                .ForMember(dest => dest.CartItemID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<CartItems, CartItemDetailsResponse>().ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart));

            //WishList
            CreateMap<CreateWishListRequest, WishList>().ForMember(dest => dest.WishlistID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<WishList, WishListResponse>();
            CreateMap<WishList, WishListDetailsResponse>().ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            //Payment
            CreateMap<PaymentRequest, Payments>().ForMember(dest => dest.PaymentID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Payments, PaymentResponse>();

            //ShippingCost
            CreateMap<CreateShippingCostRequest, ShippingCosts>().ForMember(dest => dest.ShippingCostID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<UpdateShippingCostRequest, ShippingCosts>();
            CreateMap<ShippingCosts, ShippingCostDetails>().ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area));

            //OrderCoupon
            CreateMap<CreateCouponRequest, OrderCoupons>().ForMember(dest => dest.OrderCouponID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<OrderCoupons, OrderCouponResponse>();
            CreateMap<UpdateCouponRequest, OrderCoupons>();

            //Order
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => $"ORD-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 4)}"));
            CreateMap<Order, OrderDetails>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.PersonName))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment.PaymentMethod))
                .ForMember(dest => dest.OrderCoupon, opt => opt.MapFrom(src => src.OrderCoupon))
                .ForMember(dest => dest.ShippingCosts, opt => opt.MapFrom(src => src.ShippingCosts));
            CreateMap<UpdateOrderRequest, Order>()
                .ForMember(dest => dest.PaymentID, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountAmount, opt => opt.Ignore())
                .ForMember(dest => dest.OrderCouponID, opt => opt.Ignore())
                .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
            CreateMap<Order, OrderResponse>();

            //OrderItem
            CreateMap<OrderItemRequest, OrderItems>().ForMember(r => r.OrderItemID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<CartItems, OrderItems>().ForMember(r => r.OrderItemID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(r => r.IsDeleted, opt => opt.Ignore())
                .ForMember(r => r.AddedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateOrderItemRequest, OrderItems>();
            CreateMap<OrderItems, OrderItemDetails>();

            //Banners
            CreateMap<CreateBannerRequest, BannerSlide>().ForMember(dest => dest.BannerSlideID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ImageHash, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            CreateMap<UpdateBannerRequest, BannerSlide>()
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                  .ForMember(dest => dest.ImageHash, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            CreateMap<BannerSlide, BannerSlideResponse>()
                  .ForMember(dest => dest.BackgroundColor, opt => opt.MapFrom(src => src.BackgroundColor))
                .ForMember(dest => dest.AccentColor, opt => opt.MapFrom(src => src.AccentColor));

            //Product Rates
            CreateMap<ProductRateRequest, ProductRates>().ForMember(dest => dest.RateID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
            CreateMap<ProductRates, ProductRateResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        }
    }
}
