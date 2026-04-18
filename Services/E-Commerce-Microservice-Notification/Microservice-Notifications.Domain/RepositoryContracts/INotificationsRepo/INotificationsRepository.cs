
using Microservice_Notifications.Entity;

namespace Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;

public interface INotificationsRepository
{
    public Task<Notifications?> GetNotification(Guid NotificationID);
    public Task<IEnumerable<Notifications>> GetNotifications();
    public Task<IEnumerable<Notifications>> GetUserNotifications(Guid UserID);
    public Task<IEnumerable<Notifications>> GetUserUnReadNotifications(Guid UserID);
    public Task<bool> CreateNotification(Notifications NewNotification);
    public Task<bool> SaveChanges();
}