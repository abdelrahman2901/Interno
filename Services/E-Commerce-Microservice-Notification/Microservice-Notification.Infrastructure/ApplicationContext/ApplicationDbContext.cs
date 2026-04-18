using Microservice_Notifications.Entity;
using Microsoft.EntityFrameworkCore;
namespace Microservice_Notification.Infrastructure.ApplicationContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {
        }

        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }

    }
}
