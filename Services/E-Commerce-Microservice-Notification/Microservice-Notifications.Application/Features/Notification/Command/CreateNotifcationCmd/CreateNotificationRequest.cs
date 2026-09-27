using MediatR;
using Microservice_Notifications.Domain.Common;

namespace Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd
{
    public record CreateNotificationRequest(Guid UserID,string Message) : IRequest<Result<bool>>;
}
