using AutoMapper;
using Microservice_Notification.Core.Common;
using Microservice_Notifications.Entity;
using Microservice_Notification.Core.DTO.NotificationDTO;
using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;
using Microservice_Notifications.Application.ServicesContracts.INotificationsServices;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;


namespace Microservice_Notifications.Application.Services.NotificationsServices
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsRepository _notificationsRepo;
        private readonly IMapper _Mapper;
        public NotificationsService(INotificationsRepository notificationsRepo, IMapper mapper)
        {
            _notificationsRepo = notificationsRepo;
            _Mapper = mapper;
        }

        public async Task<Result<bool>> CreateNotification(CreateNotificationRequest NewNotification)
        {
            var NewNotifeResult = await _notificationsRepo.CreateNotification(_Mapper.Map<Notifications>(NewNotification));
            if (!NewNotifeResult)
            {
                return Result<bool>.InternalError("Failed To Add Notification");
            }
            if (!await _notificationsRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChnages");
            }
            return Result<bool>.Success(NewNotifeResult);
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> GetNotifications()
        {
            var NotifesResult = await _notificationsRepo.GetNotifications();
            if (!NotifesResult.Any())
            {
                return Result<IEnumerable<NotificationResponse>>.InternalError("No Notifications Was Found");
            }

            return Result<IEnumerable<NotificationResponse>>.Success(NotifesResult.Select(s => _Mapper.Map<NotificationResponse>(s)));
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> GetUserNotifications(Guid UserID)
        {
            var NotifesResult = await _notificationsRepo.GetUserNotifications(UserID);
            if (!NotifesResult.Any())
            {
                return Result<IEnumerable<NotificationResponse>>.InternalError("No Notifications For That User Was Found");
            }

            return Result<IEnumerable<NotificationResponse>>.Success(NotifesResult.Select(s => _Mapper.Map<NotificationResponse>(s)));
        }

        public async Task<Result<bool>> MarkNotificationAsRead(Guid NotificationID)
        {
            var MarkNotfie = await _notificationsRepo.GetNotification(NotificationID);
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

        public async Task<Result<bool>> MarkNotificationAsReadList(Guid UserID)
        {
            var MarkNotfie = (await _notificationsRepo.GetUserUnReadNotifications(UserID)).ToList();
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
