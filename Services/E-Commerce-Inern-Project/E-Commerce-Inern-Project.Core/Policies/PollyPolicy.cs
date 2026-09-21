using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;

namespace E_Commerce_Inern_Project.Core.Policies
{
    public class PollyPolicy : IPollyPolicy
    {
        private readonly ILogger<PollyPolicy> _logger;

        public PollyPolicy(ILogger<PollyPolicy> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy GetPolicy()
        {
            return Policy.WrapAsync(
                GetRetryPolicy(),
                GetCircuitBreakerPolicy(),
                GetBulkheadPolicy());
        }

        private IAsyncPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryAttempt, _) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Retry {RetryAttempt} after {Delay} seconds. Reason: {Message}",
                            retryAttempt,
                            timeSpan.TotalSeconds,
                            exception.Message);
                    });
        }

        private IAsyncPolicy GetCircuitBreakerPolicy()
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                    onBreak: (exception, duration) =>
                    {
                        _logger.LogError(
                            exception,
                            "Circuit opened for {Minutes} minute(s). RabbitMQ publishes will fail fast. Reason: {Message}",
                            duration.TotalMinutes,
                            exception.Message);
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit closed. RabbitMQ publishes are allowed again.");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.LogInformation("Circuit half-open. The next publish is a trial request.");
                    });
        }

        private IAsyncPolicy GetBulkheadPolicy()
        {
            return Policy.BulkheadAsync(
                maxParallelization: 2,
                maxQueuingActions: 40, onBulkheadRejectedAsync: (context) =>
                {
                    _logger.LogWarning("Bulkhead limit reached. RabbitMQ publishes will be rejected.");
                    throw new BulkheadRejectedException("Bulkhead limit reached. RabbitMQ publishes will be rejected.");
                });
        }
    }
}
