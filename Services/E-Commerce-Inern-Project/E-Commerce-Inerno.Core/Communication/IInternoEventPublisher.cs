using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;
using E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO;

namespace E_Commerce_Inerno.Core.Communication
{
    /// <summary>
    /// Publishes audit/notification events via RabbitMQ (Docker) or HttpClient (local HTTPS).
    /// </summary>
    public interface IInternoEventPublisher
    {
        Task PublishAuditAsync(AuditRequest request);
        Task PublishNotificationAsync(CreateNotificationRequest request);
    }
}
