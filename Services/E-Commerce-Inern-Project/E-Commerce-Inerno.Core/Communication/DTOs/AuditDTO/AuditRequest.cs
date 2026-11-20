using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;

namespace E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO
{
    public record AuditRequest(Guid CreatedByUser, ActionTypeEnum ActionType, string EntityName, string? OldValues, string? NewValues);

}
