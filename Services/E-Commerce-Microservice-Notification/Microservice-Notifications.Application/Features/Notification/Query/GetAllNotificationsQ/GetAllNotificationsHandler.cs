using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notification.Core.DTO.NotificationDTO;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;

namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ
{
    public class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsQuery, Result<IEnumerable<NotificationResponse>>>
    {
        private readonly INotificationsService _notificationsService;
        public GetAllNotificationsHandler(INotificationsService service)
        {
            _notificationsService = service;
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationsService.GetNotifications();
        }
    }
}
