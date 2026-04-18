using Microservice_Audit.Domain.Entity;
using Microsoft.EntityFrameworkCore;
namespace Microservice_Audit.Infrastructure.ApplicationContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {
        }

        public DbSet<Audit> Audit { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Audit>().Property(p => p.ActionType).HasConversion<string>();
        }

    }
}
