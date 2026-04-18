using MediatR;
using Microservice_Notification.Core.Common;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;

namespace Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;

public class CreateNotificationHandler : IRequestHandler<CreateNotificationRequest, Result<bool>>
{
    private readonly INotificationsService _NotificationService;
    public CreateNotificationHandler(INotificationsService notificationService)
    {
        _NotificationService = notificationService;
    }

    public async Task<Result<bool>> Handle(CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        return await _NotificationService.CreateNotification(request);
    }
}
