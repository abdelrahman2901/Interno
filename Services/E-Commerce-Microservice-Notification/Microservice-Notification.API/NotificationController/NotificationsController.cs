using MediatR;
using Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadCmd;
using Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadListCmd;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ;
using Microservice_Notifications.Domain.Common;
using Microservice_Notifications.Domain.DTO.NotificationDTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Microservice_Notification.API.NotificationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public NotificationsController(IMediator Mediator)
        {
            _Mediator = Mediator;
        }
        
        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<NotificationResponse>>> GetAllNotifications()
        {
            return await _Mediator.Send(new GetAllNotificationsQuery());
        }
        
        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<NotificationResponse>>> GetUserNotifications()
        {
           Guid UserID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _Mediator.Send(new GetAllUserNotificationsQuery(UserID));
        }

        [HttpGet("[action]/{NotificationID}")]
        public async Task<Result<bool>> MarkNotificationAsRead(Guid NotificationID)
        {
            return await _Mediator.Send(new MarkNotificationAsReadRequest(NotificationID));
        }

        [HttpGet("[action]")]
        public async Task<Result<bool>> MarkNotificationAsReadList()
        {
            Guid UserID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _Mediator.Send(new MarkNotificationAsReadListRequest(UserID));
        }
    }
}
