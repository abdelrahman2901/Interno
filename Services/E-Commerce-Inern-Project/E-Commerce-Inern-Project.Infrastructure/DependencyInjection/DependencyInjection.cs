using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAreaRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IBannerRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICityRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ISizeRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IWishListRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using E_Commerce_Inern_Project.Infrastructure.Repository.AddressRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.AreaRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.AuthRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.BannerRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.CartItemRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.CartRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.CategoryRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.CityRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.ColorsRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.OrderCouponRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.OrderItemsRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.OrderRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.PaymentRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.ProductRatesRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.ProductRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.ShippingCostRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.SizeRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.TransectionRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.UserRepo;
using E_Commerce_Inern_Project.Infrastructure.Repository.WishListRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace E_Commerce_Inern_Project.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //color
            services.AddScoped<IColorRepository, ColorsRepository>();
            //size
            services.AddScoped<ISizeRepository, SizeRepository>();

            //Categories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            
            //Products
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductViewRepository, ProductViewRepository>();

            //Auth
            services.AddScoped<IAuthRepository, AuthRepository>();

            //User
            services.AddScoped<IUserRepository, UserRepository>();

            //area
            services.AddScoped<IAreaRepository, AreaRepository>();
            //city
            services.AddScoped<ICityRepository, CityRepository>();
            
            //Address
            services.AddScoped<IAddressRepository,AddressRepository>();

            //Cart
            services.AddScoped<ICartRepository,CartRepository>();
            
            //CartItem
            services.AddScoped<ICartItemRepository,CartItemRepository>();
            
            //WIshList
            services.AddScoped<IWishListRepository,WishListRepository>();

            //Payment
            services.AddScoped<IPaymentRepository,PaymentRepository>();

            //ShippingCost
            services.AddScoped<IShippingCostsRepository, ShippingCostRepository>();

            //Coupon
            services.AddScoped<IOrderCouponRepository, OrderCouponRepository>();

            //Order
            services.AddScoped<IOrdersRepository, OrderRepoistory>();

            //Order Item
            services.AddScoped<IOrderItemsRepoistory, OrderItemsRepoistory>();
           
            //Banner
            services.AddScoped<IBannerRepository, BannerRepository>();
            
            //Product Rating
            services.AddScoped<IProductRatesRepository, ProductRatesRepository>();


            //Transection
            services.AddScoped<ITransectionRepository,TransectionRepository>();
            //DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            return services;
        }
    }
}
