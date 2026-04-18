using Microservice_Audit.Application.RabbitMQ;
using Microsoft.Extensions.Hosting;

namespace Microservice_Audit.Application.BackGroundServices
{
    public class RabbitMqCreateAuditService : BackgroundService
    {
        private readonly IRabbitMQCreateConsumer _Consumer;
        public RabbitMqCreateAuditService(IRabbitMQCreateConsumer consumer)
        {
            _Consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _Consumer.InitAsync();
            await _Consumer.Consume();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
