using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;

namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadListCmd
{
    public class MarkNotificationAsReadListHandler : IRequestHandler<MarkNotificationAsReadListRequest, Result<bool>>
    {
        private readonly INotificationsService _notificationsService;
        public MarkNotificationAsReadListHandler(INotificationsService service)
        {
            _notificationsService = service;
        }

        public async Task<Result<bool>> Handle(MarkNotificationAsReadListRequest request, CancellationToken cancellationToken)
        {
            return await _notificationsService.MarkNotificationAsReadList(request.UserID);
        }
    }
}
