using E_Commerce_Inern_Project.Core.BackGroundServices.RabbitMQServices;
using E_Commerce_Inern_Project.Core.Features.Areas.Query.GetAllAreasQ;
using E_Commerce_Inern_Project.Core.Features.BannerSlides;
using E_Commerce_Inern_Project.Core.Features.Categories;
using E_Commerce_Inern_Project.Core.Features.Products;
using E_Commerce_Inern_Project.Core.Mapper;
using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.Services.JwtServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using E_Commerce_Inern_Project.Core.Validation.MediatorValidation;
using E_Commerce_Inern_Project.Core.Features.Products.Commands.CreateProductCommand;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace E_Commerce_Inern_Project.Core.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            //Jwt Token
            services.AddScoped<IJwtService, JwtService>();

            //Image storage, resolves wwwroot through IWebHostEnvironment
            services.AddSingleton<BannerImageStorage>();
            services.AddSingleton<CategoryImageStorage>();
            services.AddSingleton<ProductImageStorage>();

            //Fluent Validation
            services.AddValidatorsFromAssemblyContaining<ProductRequestValidation>();

            //MediatR Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //MediatR 
            services.AddMediatR(cfg =>
            {
                cfg.RegisterGenericHandlers = true;
                cfg.RegisterServicesFromAssemblyContaining<GetAllAreasHandler>();
            });

            //Mapper
            services.AddAutoMapper(sfg => { }, typeof(MapperProfile));

            //RabbitMQ
            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            // polly policies for RabbitMQ in case of connection failures, it will retry 3 times with a delay of 2 power the retry seconds between each retry
            services.AddSingleton<IPollyPolicy, PollyPolicy>();

            //background service for initializing RabbitMQ connection and channel
            services.AddHostedService<RabbitMQInitializationService>();

            string RedisConfig = $"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}";

            services.AddStackExchangeRedisCache(options => options.Configuration = RedisConfig);

            return services;
        }
    }
}
