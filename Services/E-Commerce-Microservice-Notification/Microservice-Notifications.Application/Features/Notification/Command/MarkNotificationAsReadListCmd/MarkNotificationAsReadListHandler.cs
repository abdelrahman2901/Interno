using AutoMapper;
using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;

namespace Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadListCmd
{
    public class MarkNotificationAsReadListHandler : IRequestHandler<MarkNotificationAsReadListRequest, Result<bool>>
    {
        private readonly INotificationsRepository _notificationsRepo;
        public MarkNotificationAsReadListHandler(INotificationsRepository notificationsRepo)
        {
            _notificationsRepo = notificationsRepo;
        }

        public async Task<Result<bool>> Handle(MarkNotificationAsReadListRequest request, CancellationToken cancellationToken)
        {
            var MarkNotfie = (await _notificationsRepo.GetUserUnReadNotifications(request.UserID)).ToList();
            if (!MarkNotfie.Any())
            {
                return Result<bool>.InternalError("Notifications Wasnt Found");
            }
            foreach (var notif in MarkNotfie)
            {
                notif.IsRead = true;
            }
            if (!await _notificationsRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }
            return Result<bool>.Success(MarkNotfie.Any());
        }
    }
}