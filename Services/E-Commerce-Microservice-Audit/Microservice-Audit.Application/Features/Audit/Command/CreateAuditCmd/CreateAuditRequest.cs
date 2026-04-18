using MediatR;
using Microservice_Audit.Core.Common;
using Microservice_Audit.Domain.Enum;

namespace Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd
{
    public record CreateAuditRequest(Guid CreatedByUser ,  ActionTypeEnum ActionType, string EntityName,, string? OldValues, string? NewValues) : IRequest<Result<bool>>;
}
