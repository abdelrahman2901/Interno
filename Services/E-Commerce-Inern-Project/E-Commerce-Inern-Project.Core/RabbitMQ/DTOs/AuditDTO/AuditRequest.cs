using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;

namespace E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO
{
    public record AuditRequest(Guid CreatedByUser, ActionTypeEnum ActionType, string EntityName, string? OldValues, string? NewValues);

}
