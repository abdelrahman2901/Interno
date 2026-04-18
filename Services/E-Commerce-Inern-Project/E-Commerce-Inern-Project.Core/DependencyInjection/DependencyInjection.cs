using E_Commerce_Inern_Project.Core.BackGroundServices.RabbitMQServices;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.Features.Area.Query.GetAllAreasQ;
using E_Commerce_Inern_Project.Core.Mapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.Services.AddressServices;
using E_Commerce_Inern_Project.Core.Services.AreaServices;
using E_Commerce_Inern_Project.Core.Services.AuthServices;
using E_Commerce_Inern_Project.Core.Services.BannerServices;
using E_Commerce_Inern_Project.Core.Services.CartItemServices;
using E_Commerce_Inern_Project.Core.Services.CartServices;
using E_Commerce_Inern_Project.Core.Services.CategoryServices;
using E_Commerce_Inern_Project.Core.Services.CityServices;
using E_Commerce_Inern_Project.Core.Services.ColorServices;
using E_Commerce_Inern_Project.Core.Services.JWTService;
using E_Commerce_Inern_Project.Core.Services.OrderCouponServices;
using E_Commerce_Inern_Project.Core.Services.OrderServices;
using E_Commerce_Inern_Project.Core.Services.PaymentServices;
using E_Commerce_Inern_Project.Core.Services.ProductRatesServices;
using E_Commerce_Inern_Project.Core.Services.ProductServices;
using E_Commerce_Inern_Project.Core.Services.ShippingCostsServices;
using E_Commerce_Inern_Project.Core.Services.SizeServices;
using E_Commerce_Inern_Project.Core.Services.UserServices;
using E_Commerce_Inern_Project.Core.Services.WishListServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAddressServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IBannerServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICategoryServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderCouponServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IPaymentServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using E_Commerce_Inern_Project.Core.Validation.MediatorValidation;
using E_Commerce_Inern_Project.Core.Validation.ProductValidation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace E_Commerce_Inern_Project.Core.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services,IConfiguration configuration)
        {
            //color
            services.AddScoped<IColorService, ColorService>();

            //size
            services.AddScoped<ISizeService, SizeService>();

            //Categories
            services.AddScoped<ICategoryService, CategoryService>();
            //Products
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductViewService, ProductViewService>();
            //Auth
            services.AddScoped<IAuthService, AuthService>();
            //User
            services.AddScoped<IUserService, UserService>();

            //Jwt Token
            services.AddScoped<IJwtService, JwtService>();

            //Fluent Validation
            services.AddValidatorsFromAssemblyContaining<ProductRequestValidation>();

            //MediatR Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //Area
            services.AddScoped<IAreaService,AreaService>();
            //City
            services.AddScoped<ICityService,CityService>();

            //Address
            services.AddScoped<IAddressService, AddressService>();

            //Cart
            services.AddScoped<ICartService, CartService>();

            //CartItem
            services.AddScoped<ICartItemService, CartItemService>();

            //WIshList

            services.AddScoped<IWishListService, WishListService>();
            //Payments
            services.AddScoped<IPaymentService, PaymentService>();
            
            //ShippingCost
            services.AddScoped<IShippingCostsService, ShippingCostsService>();
            //Coupon
            services.AddScoped<IOrderCouponService, OrderCouponService>();

            //Order
            services.AddScoped<IOrderService, OrderService>();

            //banner 
            services.AddScoped<IBannerService, BannerService>();

            //productRating
            services.AddScoped<IProductRatesService, ProductRatesService>();

            //MediatR 
            services.AddMediatR(cfg =>
            {
                cfg.RegisterGenericHandlers = true;
                cfg.RegisterServicesFromAssemblyContaining<GetAllAreasHandler>();
            });

            //Mapper
            services.AddAutoMapper(sfg => { }, typeof(MapperProfile));

            //RabbitMQ
            services.AddSingleton<IRabbitMQPublisher,RabbitMQPublisher>();

            // polly policies for RabbitMQ in case of connection failures, it will retry 3 times with a delay of 2 seconds between each retry
            services.AddSingleton<IAsyncPolicy>(Policy.Handle<Exception>().WaitAndRetryAsync(3,retry=>TimeSpan.FromSeconds(2)));

            //background service for initializing RabbitMQ connection and channel
            services.AddHostedService<RabbitMQInitializationService>();

            string RedisConfig = $"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}";
          
            services.AddStackExchangeRedisCache(options => options.Configuration = RedisConfig);

            return services;
        }
    }
}
