using Microservice_Audit.Domain.Entity;

namespace Microservice_Audit.Domain.RepositoryContracts.IAuditsRepo
{
    public interface IAuditRepository
    {
        public Task<Audit?> GetAudit(Guid AuditID);
        public Task<IEnumerable<Audit>> GetAudits();
        public Task<bool> CreateAudit(Audit NewAudit);
        public Task<bool> SaveChanges();
    }
}