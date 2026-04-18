using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notification.Core.DTO.NotificationDTO;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;

namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ
{
    public class GetAllUserNotificationsHandler : IRequestHandler<GetAllUserNotificationsQuery, Result<IEnumerable<NotificationResponse>>>
    {
        private readonly INotificationsService _notificationsService;
        public GetAllUserNotificationsHandler(INotificationsService service)
        {
            _notificationsService = service;
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> Handle(GetAllUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationsService.GetUserNotifications(request.UserID);
        }
    }
}
