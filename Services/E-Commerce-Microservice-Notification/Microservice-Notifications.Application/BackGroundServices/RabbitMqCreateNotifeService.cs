using Microservice_Notifications.Application.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microservice_Notifications.Application.BackGroundServices
{
    public class RabbitMqCreateNotifeService : BackgroundService
    {
        private readonly IRabbitMQCreateConsumer _Consumer;
        private readonly ILogger<RabbitMqCreateNotifeService> _logger;
        private readonly bool _runningInDocker =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public RabbitMqCreateNotifeService(IRabbitMQCreateConsumer consumer, ILogger<RabbitMqCreateNotifeService> logger)
        {
            _Consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Local HTTPS mode receives notifications via HTTP CreateNotification endpoint instead of RabbitMQ.
            if (!_runningInDocker)
            {
                _logger.LogInformation("Not running in Docker. Skipping RabbitMQ notification consumer (HttpClient mode).");
                return;
            }

            await _Consumer.InitAsync();
            await _Consumer.Consume();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
