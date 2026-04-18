using FluentValidation;
using MediatR;
using Microservice_Notifications.Application.RabbitMQ;
using Microservice_Notifications.Application.BackGroundServices;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ;
using Microservice_Notifications.Application.Mapper;
using Microservice_Notifications.Application.Validation.NotificationValidation;
using Microservice_Notifications.Application.Validation.ValidationPipeLine;
using Microsoft.Extensions.DependencyInjection;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;
using Microservice_Notifications.Application.Services.NotificationsServices;

namespace Microservice_Notifications.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection Services)
        {

            Services.AddValidatorsFromAssemblyContaining<CreateNotificationRequestValidation>();

            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            Services.AddAutoMapper(cng => { }, typeof(MapperProfile));


            Services.AddScoped<INotificationsService,NotificationsService>();
            //RabbitMQ
            Services.AddTransient<IRabbitMQCreateConsumer, RabbitMQCreateConsumer>();

            //BackGroundService
            Services.AddHostedService<RabbitMqCreateNotifeService>();
            return Services;
        }
    }
}
