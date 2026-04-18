using MediatR;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.DTO.AuditDTO;
using Microservice_Audit.Core.ServicesContracts.IAuditsServices;

namespace Microservice_Audit.Application.Features.Audit.Query.GetAllAuditQ
{
    public class GetAllAuditHandler : IRequestHandler<GetAllAuditQuery, Result<IEnumerable<AuditResponse>>>
    {
        private readonly IAuditService _notificationsService;
        public GetAllAuditHandler(IAuditService service)
        {
            _notificationsService = service;
        }

        public async Task<Result<IEnumerable<AuditResponse>>> Handle(GetAllAuditQuery request, CancellationToken cancellationToken)
        {
            return await _notificationsService.GetAudits();
        }
    }
}
