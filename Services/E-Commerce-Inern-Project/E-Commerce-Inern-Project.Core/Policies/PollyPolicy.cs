using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace E_Commerce_Inern_Project.Core.Policies
{
    public class PollyPolicy : IPollyPolicy
    {
        private readonly ILogger<PollyPolicy> _logger;
        public PollyPolicy(ILogger<PollyPolicy> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            IAsyncPolicy<HttpResponseMessage> policy =
       HttpPolicyExtensions
           .HandleTransientHttpError()
           .WaitAndRetryAsync(
               retryCount: 3,
               sleepDurationProvider: retry =>
                   TimeSpan.FromSeconds(Math.Pow(2, retry)),
               onRetry: (outcome, timespan, retryAttempt, context) =>
               {
                   _logger.LogInformation(
                       "Retry {RetryAttempt} after {Delay} seconds",
                       retryAttempt,
                       timespan.TotalSeconds);
               });

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCircuiteBreakerPolicy()
        {
            AsyncCircuitBreakerPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromMinutes(1), onBreak: (outcome, timespan) =>
                {
                    _logger.LogInformation($"Circuit breaker opened for {timespan.TotalMinutes} minutes due to consecutive 3 failures. The subsequent requests will be blocked");
                },
                onReset: () =>
                {
                    _logger.LogInformation($"Circuit breaker closed. The subsequent requests will be allowed.");
                });
            return policy;
        }
    }
}