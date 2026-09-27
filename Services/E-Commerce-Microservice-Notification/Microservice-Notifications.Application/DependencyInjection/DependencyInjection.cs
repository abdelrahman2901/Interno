using FluentValidation;
using MediatR;
using Microservice_Notifications.Application.RabbitMQ;
using Microservice_Notifications.Application.BackGroundServices;
using Microservice_Notifications.Application.Mapper;
using Microservice_Notifications.Application.Validation.NotificationValidation;
using Microservice_Notifications.Application.Validation.ValidationPipeLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Microservice_Notifications.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection Services,IConfiguration configuration)
        {

            Services.AddValidatorsFromAssemblyContaining<CreateNotificationRequestValidation>();

            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            Services.AddAutoMapper(cng => { }, typeof(MapperProfile));

            var RedisConfiguration = $"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}";

            //RabbitMQ
            Services.AddTransient<IRabbitMQCreateConsumer, RabbitMQCreateConsumer>();

            //BackGroundService
            Services.AddHostedService<RabbitMqCreateNotifeService>();
            return Services;
        }
    }
}
