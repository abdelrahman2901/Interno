using E_Commerce_Inerno.Core.Communication.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Core.BackGroundServices.RabbitMQServices
{
    public class RabbitMQInitializationService : BackgroundService
    {
        private const int MaxAttempts = 5;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(2);
        private readonly IRabbitMQPublisher _Publisher;
        private readonly ILogger<RabbitMQInitializationService> _logger;
        private readonly bool _runningInDocker;

        public RabbitMQInitializationService(IRabbitMQPublisher Publisher, ILogger<RabbitMQInitializationService> logger)
        {
            _Publisher = Publisher;
            _logger = logger;
            _runningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Local HTTPS mode uses HttpClient — skip RabbitMQ entirely.
            if (!_runningInDocker)
            {
                _logger.LogInformation("Not running in Docker. Skipping RabbitMQ initialization (HttpClient mode).");
                return;
            }

            Exception? lastError = null;

            for (int attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                stoppingToken.ThrowIfCancellationRequested();

                try
                {
                    await _Publisher.InitAsync();
                    _logger.LogInformation("RabbitMQ initialized successfully.");
                    lastError = null;
                    break;
                }
                catch (Exception ex) when (attempt < MaxAttempts)
                {
                    lastError = ex;
                    _logger.LogWarning(ex, "RabbitMQ init failed (attempt {Attempt}/{MaxAttempts}). Retrying in {DelaySeconds}s.", attempt, MaxAttempts, RetryDelay.TotalSeconds);
                    await Task.Delay(RetryDelay, stoppingToken);
                }
                catch (Exception ex)
                {
                    lastError = ex;
                }
            }

            if (lastError != null)
            {
                throw lastError;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
