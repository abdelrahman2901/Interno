
using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.DTO.AuditDTO;

namespace Microservice_Audit.Core.ServicesContracts.IAuditsServices
{
    public interface IAuditService
    {
        public Task<Result<IEnumerable<AuditResponse>>> GetAudits(); 
        public Task<Result<bool>> CreateAudit(CreateAuditRequest NewAudit);
   
    }
}
