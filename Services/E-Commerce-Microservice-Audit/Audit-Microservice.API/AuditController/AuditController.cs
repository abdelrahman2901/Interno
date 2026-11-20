using MediatR;
using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;
using Microservice_Audit.Application.Features.Audit.Query.GetAllAuditQ;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Core.DTO.AuditDTO;
using Microsoft.AspNetCore.Mvc;

namespace Audit_Microservice.API.AuditController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<AuditResponse>>> GetAllAudit()
        {
            return await _mediator.Send(new GetAllAuditQuery());
        }

        [HttpPost("[action]")]
        public async Task<Result<bool>> CreateAudit(CreateAuditRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}