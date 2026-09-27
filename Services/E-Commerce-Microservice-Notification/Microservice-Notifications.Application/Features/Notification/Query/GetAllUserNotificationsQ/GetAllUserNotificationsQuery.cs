using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.DTO.NotificationDTO;


namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ
{
    public record GetAllUserNotificationsQuery(Guid UserID) : IRequest<Result<IEnumerable<NotificationResponse>>>;
}
