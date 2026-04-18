using MediatR;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.DTO.AuditDTO;


namespace Microservice_Audit.Application.Features.Audit.Query.GetAllAuditQ
{
    public record GetAllAuditQuery() : IRequest<Result<IEnumerable<AuditResponse>>>;
    
}
