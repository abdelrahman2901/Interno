using E_Commerce_Inerno.Core.Features.Areas.Query.GetAllAreasQ;
using E_Commerce_Inerno.Core.Features.BannerSlides;
using E_Commerce_Inerno.Core.Features.Categories;
using E_Commerce_Inerno.Core.Mapper;
using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Services.JwtServices;
using E_Commerce_Inerno.Core.ServicesContracts.IJWTServices;
using E_Commerce_Inerno.Core.Validation.MediatorValidation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using E_Commerce_Inerno.Core.Features.Products.Commands.CreateProductCommand;
using E_Commerce_Inerno.Core.BackGroundServices.RabbitMQServices;
using E_Commerce_Inerno.Core.Features.Products;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.Communication.RabbitMQ;


namespace E_Commerce_Inerno.Core.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            var runningInDocker =
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

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

            //RabbitMQ (used only in Docker; local HTTPS uses HttpClient via IInternoEventPublisher)
            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            // polly policies for RabbitMQ / HttpClient failures
            services.AddSingleton<IPollyPolicy, PollyPolicy>();

            // Dual-mode publisher: RabbitMQ in Docker, HttpClient locally
            services.AddScoped<IInternoEventPublisher, InternoEventPublisher>();

            //background service for initializing RabbitMQ connection and channel
            services.AddHostedService<RabbitMQInitializationService>();

            // Redis in Docker; in-memory cache locally (avoids needing Redis when RAM is tight)
            if (runningInDocker)
            {
                string RedisConfig = $"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}";
                services.AddStackExchangeRedisCache(options => options.Configuration = RedisConfig);
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            return services;
        }
    }
}
