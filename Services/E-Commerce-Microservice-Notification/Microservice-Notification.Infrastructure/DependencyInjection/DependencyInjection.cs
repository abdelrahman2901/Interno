using Microservice_Notification.Infrastructure.ApplicationContext;
using Microservice_Notification.Infrastructure.Repository.NotificationsRepo;
using Microservice_Notifications.Domain.RepositoryContracts.INotificationsRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice_Notification.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection Service,IConfiguration configuration)
        {
            Service.AddScoped<INotificationsRepository, NotificationsRepository>();

            Service.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            return Service;
        }
    }
}
