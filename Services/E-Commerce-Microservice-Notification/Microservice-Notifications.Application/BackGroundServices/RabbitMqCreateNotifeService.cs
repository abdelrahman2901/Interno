using Microservice_Notifications.Application.RabbitMQ;
using Microsoft.Extensions.Hosting;

namespace Microservice_Notifications.Application.BackGroundServices
{
    public class RabbitMqCreateNotifeService : BackgroundService
    {
        private readonly IRabbitMQCreateConsumer _Consumer;
        public RabbitMqCreateNotifeService(IRabbitMQCreateConsumer consumer)
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
