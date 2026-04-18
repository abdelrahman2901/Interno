using Microservice_Audit.Domain.RepositoryContracts.IAuditsRepo;
using Microservice_Audit.Infrastructure.ApplicationContext;
using Microservice_Audit.Infrastructure.Repository.AuditRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice_Audit.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection Service,IConfiguration configuration)
        {
            Service.AddScoped<IAuditRepository, AuditRepository>();

            Service.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            return Service;
        }
    }
}
