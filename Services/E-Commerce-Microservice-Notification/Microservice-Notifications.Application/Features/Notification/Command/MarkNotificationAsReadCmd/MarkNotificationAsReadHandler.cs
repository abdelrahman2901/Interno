using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;

namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadCmd
{
    public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsReadRequest, Result<bool>>
    {
        private readonly INotificationsRepository _notificationsRepo;
        public MarkNotificationAsReadHandler(INotificationsRepository notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        public async Task<Result<bool>> Handle(MarkNotificationAsReadRequest request, CancellationToken cancellationToken)
        {
            var MarkNotfie = await _notificationsRepo.GetNotification(request.NotificationID);
            if (MarkNotfie == null)
            {
                return Result<bool>.InternalError("Notification Wasnt Found");
            }
            
            MarkNotfie.IsRead = true;

            if (!await _notificationsRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }
            return Result<bool>.Success(MarkNotfie.IsRead);
        }
    }
}
