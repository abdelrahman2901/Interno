using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.DTO.NotificationDTO;


namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ
{
    public record GetAllNotificationsQuery() : IRequest<Result<IEnumerable<NotificationResponse>>>;
    
}
