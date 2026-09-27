namespace E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.NotificationsDTO
{
    public record CreateNotificationRequest(Guid UserID, string Message);

}
