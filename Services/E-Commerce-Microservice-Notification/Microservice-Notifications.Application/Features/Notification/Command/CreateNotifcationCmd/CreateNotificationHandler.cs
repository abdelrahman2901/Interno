using AutoMapper;
using MediatR;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;
using Microservice_Notifications.Entity;

namespace Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;

public class CreateNotificationHandler : IRequestHandler<CreateNotificationRequest, Result<bool>>
{
    private readonly INotificationsRepository _notificationsRepo;
    private readonly IMapper _Mapper;
    public CreateNotificationHandler(INotificationsRepository notificationsRepo, IMapper mapper)
    {
        _notificationsRepo = notificationsRepo;
        _Mapper = mapper;
    }

    public async Task<Result<bool>> Handle(CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var NewNotifeResult = await _notificationsRepo.CreateNotification(_Mapper.Map<Notifications>(request));
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
}
