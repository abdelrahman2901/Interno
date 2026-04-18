using Microservice_Notification.Infrastructure.ApplicationContext;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;
using Microservice_Notifications.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Microservice_Notification.Infrastructure.Repository.NotificationsRepo
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly ILogger<NotificationsRepository> _logger;
        public NotificationsRepository(ApplicationDbContext context,ILogger<NotificationsRepository>logger)
        {
            _Context = context;
            _logger = logger;
        }

        public async Task<bool> CreateNotification(Notifications NewNotification)
        {
            try
            {
                await _Context.Notifications.AddAsync(NewNotification);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In CreateNotification Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                _logger.LogError("Error Occured In CreateNotification InnerException  : {message}", ex.InnerException.Message);
                }
                return false;
            }
            
        }

        public async Task<Notifications?> GetNotification(Guid NotificationID)
        {
            try
            {
                return await _Context.Notifications.FirstOrDefaultAsync(g=>g.NotificationID==NotificationID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetNotification Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetNotification InnerException  : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<Notifications>> GetNotifications()
        {
            try
            {
                return await _Context.Notifications.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetNotifications Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetNotifications InnerException  : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<IEnumerable<Notifications>> GetUserNotifications(Guid UserID)
        {
            try
            {
                return await _Context.Notifications.AsNoTracking().Where(r=>r.UserID==UserID).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetUserNotifications Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetUserNotifications InnerException  : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }
        public async Task<IEnumerable<Notifications>> GetUserUnReadNotifications(Guid UserID)
        {
            try
            {
                return await _Context.Notifications.Where(r=>r.UserID==UserID &&!r.IsRead).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetUserNotifications Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetUserNotifications InnerException  : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In SaveChanges Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In SaveChanges InnerException  : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
