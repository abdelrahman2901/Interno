using Microservice_Audit.Domain.Entity;
using Microservice_Audit.Domain.RepositoryContracts.IAuditsRepo;
using Microservice_Audit.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Microservice_Audit.Infrastructure.Repository.AuditRepo
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly ILogger<AuditRepository> _logger;
        public AuditRepository(ApplicationDbContext context,ILogger<AuditRepository>logger)
        {
            _Context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAudit(Audit NewAudit)
        {
            try
            {
                await _Context.Audit.AddAsync(NewAudit);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In CreateAudit Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                _logger.LogError("Error Occured In CreateAudit InnerException  : {message}", ex.InnerException.Message);
                }
                return false;
            }
            
        }

        public async Task<Audit?> GetAudit(Guid AuditID)
        {
            try
            {
                return await _Context.Audit.FirstOrDefaultAsync(g=>g.AuditID==AuditID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetAudit Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetAudit InnerException  : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<Audit>> GetAudits()
        {
            try
            {
                return await _Context.Audit.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured In GetAudits Exception  : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured In GetAudits InnerException  : {message}", ex.InnerException.Message);
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
