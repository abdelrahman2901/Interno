namespace E_Commerce_Inerno.Core.Communication.DTOs.NotificationsDTO
{
    public record CreateNotificationRequest(Guid UserID, string Message);

}
