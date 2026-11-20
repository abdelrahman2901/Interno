using Microservice_Audit.Application.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microservice_Audit.Application.BackGroundServices
{
    public class RabbitMqCreateAuditService : BackgroundService
    {
        private readonly IRabbitMQCreateConsumer _Consumer;
        private readonly ILogger<RabbitMqCreateAuditService> _logger;
        private readonly bool _runningInDocker =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public RabbitMqCreateAuditService(IRabbitMQCreateConsumer consumer, ILogger<RabbitMqCreateAuditService> logger)
        {
            _Consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Local HTTPS mode receives audits via HTTP CreateAudit endpoint instead of RabbitMQ.
            if (!_runningInDocker)
            {
                _logger.LogInformation("Not running in Docker. Skipping RabbitMQ audit consumer (HttpClient mode).");
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
