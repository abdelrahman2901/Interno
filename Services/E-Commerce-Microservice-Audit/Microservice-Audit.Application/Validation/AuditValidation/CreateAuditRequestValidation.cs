using FluentValidation;
using Microservice_Audit.Application.Features.Audit.Command.CreateAuditCmd;

namespace Microservice_Audit.Application.Validation.AuditValidation
{
    public class CreateAuditRequestValidation : AbstractValidator<CreateAuditRequest>
    {
        public CreateAuditRequestValidation()
        {
            RuleFor(p => p.CreatedByUser).NotEmpty().WithMessage(" Cant Be Empty");
            RuleFor(p => p.ActionType).IsInEnum().WithMessage("Invalid Enum For ActionTyoe ");
        }
    }
}
