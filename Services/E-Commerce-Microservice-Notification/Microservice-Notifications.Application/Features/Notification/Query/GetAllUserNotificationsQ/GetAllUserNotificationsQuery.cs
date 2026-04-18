using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notification.Core.DTO.NotificationDTO;


namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ
{
    public record GetAllUserNotificationsQuery(Guid UserID) : IRequest<Result<IEnumerable<NotificationResponse>>>;
}
