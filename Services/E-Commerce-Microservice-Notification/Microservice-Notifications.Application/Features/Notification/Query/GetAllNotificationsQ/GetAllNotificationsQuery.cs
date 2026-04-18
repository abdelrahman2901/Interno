using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notification.Core.DTO.NotificationDTO;


namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ
{
    public record GetAllNotificationsQuery() : IRequest<Result<IEnumerable<NotificationResponse>>>;
    
}
