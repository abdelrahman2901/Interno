using System.ComponentModel.DataAnnotations;

namespace Microservice_Notifications.Entity
{
    public class Notifications
    {
        [Key]
        public Guid NotificationID { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }=false;
        public DateTime CreatedAt { get; set; }
    }
}
