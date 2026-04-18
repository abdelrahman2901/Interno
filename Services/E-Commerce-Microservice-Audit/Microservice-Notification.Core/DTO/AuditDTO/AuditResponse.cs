
namespace Microservice_Audit.Core.DTO.AuditDTO
{
    public class AuditResponse
    {
        public Guid AuditID { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid UpdatedByUser { get; set; }
        public string ActionType { get; set; }
        public string EntityName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? OldValues { get; set; } // JSON representation of old values Note: incase Null its new value 
        public string? NewValues { get; set; } // JSON representation of new values Note incase null its deleted
    }
}
