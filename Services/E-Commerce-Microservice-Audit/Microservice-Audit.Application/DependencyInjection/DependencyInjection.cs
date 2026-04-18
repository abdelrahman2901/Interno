using FluentValidation;
using MediatR;
using Microservice_Audit.Application.BackGroundServices;
using Microservice_Audit.Application.Mapper;
using Microservice_Audit.Application.RabbitMQ;
using Microservice_Audit.Application.Services.AuditServices;
using Microservice_Audit.Application.Validation.AuditValidation;
using Microservice_Audit.Application.Validation.ValidationPipeLine;
using Microservice_Audit.Core.ServicesContracts.IAuditsServices;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice_Audit.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection Services)
        {
            Services.AddScoped<IAuditService, AuditService>();

            Services.AddValidatorsFromAssemblyContaining<CreateAuditRequestValidation>();

            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            Services.AddAutoMapper(cng => { }, typeof(MapperProfile));

            //RabbitMQ
            Services.AddTransient<IRabbitMQCreateConsumer, RabbitMQCreateConsumer>();

            //BackGroundService
            Services.AddHostedService<RabbitMqCreateAuditService>();
            return Services;
        }
    }
}
