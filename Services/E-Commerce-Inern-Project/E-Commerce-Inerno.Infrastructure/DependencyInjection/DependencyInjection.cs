using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IUserRolesRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ICartItemRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrderCouponRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRatesRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.ISizeRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Inerno.Infrastructure.Repository.AddressRepo;
using E_Commerce_Inerno.Infrastructure.Repository.AreaRepo;
using E_Commerce_Inerno.Infrastructure.Repository.AuthRepo;
using E_Commerce_Inerno.Infrastructure.Repository.BannerRepo;
using E_Commerce_Inerno.Infrastructure.Repository.CartItemRepo;
using E_Commerce_Inerno.Infrastructure.Repository.CartRepo;
using E_Commerce_Inerno.Infrastructure.Repository.CategoryRepo;
using E_Commerce_Inerno.Infrastructure.Repository.CityRepo;
using E_Commerce_Inerno.Infrastructure.Repository.ColorsRepo;
using E_Commerce_Inerno.Infrastructure.Repository.OrderCouponRepo;
using E_Commerce_Inerno.Infrastructure.Repository.OrderItemsRepo;
using E_Commerce_Inerno.Infrastructure.Repository.OrderRepo;
using E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo;
using E_Commerce_Inerno.Infrastructure.Repository.ProductRatesRepo;
using E_Commerce_Inerno.Infrastructure.Repository.ProductRepo;
using E_Commerce_Inerno.Infrastructure.Repository.ShippingCostRepo;
using E_Commerce_Inerno.Infrastructure.Repository.SizeRepo;
using E_Commerce_Inerno.Infrastructure.Repository.TransectionRepo;
using E_Commerce_Inerno.Infrastructure.Repository.UserRepo;
using E_Commerce_Inerno.Infrastructure.Repository.UserRolesRepo;
using E_Commerce_Inerno.Infrastructure.Repository.WishListRepo;
using E_Commerce_Inerno.Infrastructure.Repository.ProductRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace E_Commerce_Inerno.Infrastructure.DependencyInjection
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
            services.AddScoped<IAddressRepository, AddressRepository>();

            //Cart
            services.AddScoped<ICartRepository, CartRepository>();

            //CartItem
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            //WIshList
            services.AddScoped<IWishListRepository, WishListRepository>();

            //Payment
            services.AddScoped<IPaymentRepository, PaymentRepository>();

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

            //UserRoles
            services.AddScoped<IUserRolesRepository, UserRolesRepository>();

            //Transection
            services.AddScoped<ITransectionRepository, TransectionRepository>();
            //DbContext
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                var runningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
                var ConnectionString = runningInDocker ? configuration.GetConnectionString("DockerConnection") : configuration.GetConnectionString("Default");
                Console.WriteLine(runningInDocker);
                Console.WriteLine(ConnectionString);
                options.UseSqlServer(ConnectionString);
            });

            return services;
        }
    }
}
