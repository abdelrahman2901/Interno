using AutoMapper;
using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.DTO.NotificationDTO;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;

namespace Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ
{
    public class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsQuery, Result<IEnumerable<NotificationResponse>>>
    {
        private readonly INotificationsRepository _notificationsRepo;
        private readonly IMapper _Mapper;
        public GetAllNotificationsHandler(INotificationsRepository notificationsRepo, IMapper mapper)
        {
            _notificationsRepo = notificationsRepo;
            _Mapper = mapper;
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            var NotifesResult = await _notificationsRepo.GetNotifications();
            if (!NotifesResult.Any())
            {
                return Result<IEnumerable<NotificationResponse>>.InternalError("No Notifications Was Found");
            }
      

            return Result<IEnumerable<NotificationResponse>>.Success(NotifesResult.Select(s => _Mapper.Map<NotificationResponse>(s)));
        }
    }
}
