using MediatR;
using Microservice_Notification.Core.Common;


namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadCmd
{
    public record MarkNotificationAsReadRequest(Guid NotificationID) : IRequest<Result<bool>>;
  
}
