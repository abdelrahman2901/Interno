using E_Commerce_Inerno.Core.Policies;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO;
using E_Commerce_Inerno.Core.Communication.HttpClients;
using E_Commerce_Inerno.Core.Communication.RabbitMQ;
using Microsoft.Extensions.Logging;
using Polly;

namespace E_Commerce_Inerno.Core.Communication
{
    public class InternoEventPublisher : IInternoEventPublisher
    {
        private const string AuditRoutingKey = "Interno.Audit";
        private const string NotificationRoutingKey = "Interno.Notification";

        private readonly IRabbitMQPublisher _rabbitMqPublisher;
        private readonly CreateAuditClient _auditClient;
        private readonly CreateNotificationClient _notificationClient;
        private readonly IAsyncPolicy _policy;
        private readonly ILogger<InternoEventPublisher> _logger;
        private readonly bool _runningInDocker =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        public InternoEventPublisher(
            IRabbitMQPublisher rabbitMqPublisher,
            CreateAuditClient auditClient,
            CreateNotificationClient notificationClient,
            IPollyPolicy pollyPolicy,
            ILogger<InternoEventPublisher> logger)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _auditClient = auditClient;
            _notificationClient = notificationClient;
            _policy = pollyPolicy.GetPolicy();
            _logger = logger;
        }

        public async Task PublishAuditAsync(AuditRequest request)
        {
            if (_runningInDocker)
            {
                await _policy.ExecuteAsync(async () =>
                {
                    await _rabbitMqPublisher.Publish(AuditRoutingKey, request);
                });
                return;
            }

            var result = await _policy.ExecuteAsync(async () =>
                await _auditClient.CreateAuditLogAsync(request));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("HttpClient audit publish failed: {Error}", result.ErrorMessage);
            }
        }

        public async Task PublishNotificationAsync(CreateNotificationRequest request)
        {
            if (_runningInDocker)
            {
                await _policy.ExecuteAsync(async () =>
                {
                    await _rabbitMqPublisher.Publish(NotificationRoutingKey, request);
                });
                return;
            }

            var result = await _policy.ExecuteAsync(async () =>
                await _notificationClient.CreateNotificationAsync(request));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("HttpClient notification publish failed: {Error}", result.ErrorMessage);
            }
        }
    }
}
