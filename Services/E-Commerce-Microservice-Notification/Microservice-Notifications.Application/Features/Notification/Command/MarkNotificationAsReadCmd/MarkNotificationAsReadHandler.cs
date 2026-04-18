using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;

namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadCmd
{
    public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsReadRequest, Result<bool>>
    {
        private readonly INotificationsService _notificationsService;
        public MarkNotificationAsReadHandler(INotificationsService service)
        {
            _notificationsService = service;
        }

        public async Task<Result<bool>> Handle(MarkNotificationAsReadRequest request, CancellationToken cancellationToken)
        {
            return await _notificationsService.MarkNotificationAsRead(request.NotificationID);
        }
    }
}
