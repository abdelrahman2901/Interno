using MediatR;
using Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadCmd;
using Microservice_Notifications.Application.Features.Notification.Command.MarkNotificationAsReadListCmd;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllNotificationsQ;
using Microservice_Notifications.Application.Features.Notification.Query.GetAllUserNotificationsQ;


namespace Microservice_Notification.API.Endpoints
{
    public static class NotificaitonEndPoint
    {

        public static void MapNotificationEndPoint(this IEndpointRouteBuilder app)
        {

            app.MapGet("/api/Notificaiton/GetNotification", async (IMediator _Medaitor) =>
            {
                return await _Medaitor.Send(new GetAllNotificationsQuery());

            });

            app.MapGet("/api/Notificaiton/GetUserNotifications/{UserID}", async (IMediator _Medaitor, Guid UserID) =>
            {
                return await _Medaitor.Send(new GetAllUserNotificationsQuery(UserID));
            });

            app.MapPut("/api/Notificaiton/MarkNotificationAsRead/{NotificationID}", async (IMediator _Medaitor, Guid NotificationID) =>
            {
                return await _Medaitor.Send(new MarkNotificationAsReadRequest(NotificationID));
            });

            app.MapPut("/api/Notificaiton/MarkNotificationAsReadList/{UserID}", async (IMediator _Medaitor, Guid UserID) =>
            {
                return await _Medaitor.Send(new MarkNotificationAsReadListRequest(UserID));
            });
        }
    }
}
