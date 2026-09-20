using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

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
            var policy = Policy.WrapAsync(GetRetryPolicy(), GetCircuiteBreakerPolicy());
            return policy;
        }

        private IAsyncPolicy GetRetryPolicy()
        {
         return   Policy
          .Handle<Exception>()
          .WaitAndRetryAsync(
              3,
              retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
              onRetry: (exception, timespan, retryAttempt, context) =>
              {
                  _logger.LogInformation(
                      "Retry {RetryAttempt} after {Delay} seconds",
                      retryAttempt,
                      timespan.TotalSeconds);
              });
        }

        private IAsyncPolicy GetCircuiteBreakerPolicy()
        {
            return  Policy.Handle<Exception>().CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3, 
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogInformation($"Circuit breaker opened for {timespan.TotalMinutes} minutes due to consecutive 3 failures. The subsequent requests will be blocked");
                },
                onReset: () =>
                {
                    _logger.LogInformation($"Circuit breaker closed. The subsequent requests will be allowed.");
                });
        }
    }
}