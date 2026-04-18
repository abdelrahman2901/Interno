using MediatR;
using Microservice_Notification.Core.Common;
 
namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadListCmd
{
    public record MarkNotificationAsReadListRequest(Guid UserID) : IRequest<Result<bool>>;
   
}
