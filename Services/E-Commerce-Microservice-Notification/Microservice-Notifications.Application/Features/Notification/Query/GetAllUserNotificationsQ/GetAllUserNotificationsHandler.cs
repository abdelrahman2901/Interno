using AutoMapper;
using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.DTO.NotificationDTO;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;

namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ
{
    public class GetAllUserNotificationsHandler : IRequestHandler<GetAllUserNotificationsQuery, Result<IEnumerable<NotificationResponse>>>
    {
        private readonly INotificationsRepository _notificationsRepo;
        private readonly IMapper _Mapper;
        public GetAllUserNotificationsHandler(INotificationsRepository notificationsRepo, IMapper mapper)
        {
            _notificationsRepo = notificationsRepo;
            _Mapper = mapper;
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> Handle(GetAllUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var NotifesResult = await _notificationsRepo.GetUserNotifications(request.UserID);
            if (!NotifesResult.Any())
            {
                return Result<IEnumerable<NotificationResponse>>.InternalError("No Notifications For That User Was Found");
            }

            return Result<IEnumerable<NotificationResponse>>.Success(NotifesResult.Select(s => _Mapper.Map<NotificationResponse>(s)));
        }
    }
}
