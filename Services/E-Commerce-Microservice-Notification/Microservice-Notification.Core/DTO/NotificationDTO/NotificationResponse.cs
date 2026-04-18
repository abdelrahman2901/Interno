namespace Microservice_Notification.Core.DTO.NotificationDTO
{
    public class NotificationResponse
    {
        public Guid NotificationID { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
