using MediatR;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.ServicesContracts.IAuditsServices;


namespace Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd
{
    public class CreateAuditHandler : IRequestHandler<CreateAuditRequest, Result<bool>>
    {
        private readonly IAuditService _NotificationService;
        public CreateAuditHandler(IAuditService notificationService)
        {
            _NotificationService = notificationService;
        }

        public async Task<Result<bool>> Handle(CreateAuditRequest request, CancellationToken cancellationToken)
        {
            return await _NotificationService.CreateAudit(request);
        }
    }
}
