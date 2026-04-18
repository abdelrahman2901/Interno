using Microservice_Notification.Core.Common;
using Microservice_Notification.Core.DTO.NotificationDTO;
using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;

namespace Microservice_Notifications.Application.ServicesContracts.INotificationsServices
{
    public interface INotificationsService
    {
        public Task<Result<IEnumerable<NotificationResponse>>> GetNotifications();
        public Task<Result<IEnumerable<NotificationResponse>>> GetUserNotifications(Guid UserID);
        public Task<Result<bool>> CreateNotification(CreateNotificationRequest NewNotification);
        public Task<Result<bool>> MarkNotificationAsRead(Guid NotificationID);
        public Task<Result<bool>> MarkNotificationAsReadList(Guid UserID);
    }
}
