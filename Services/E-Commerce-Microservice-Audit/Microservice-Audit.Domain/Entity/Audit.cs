using Microservice_Audit.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Microservice_Audit.Domain.Entity
{
    public class Audit
    {
        [Key]
        public Guid AuditID { get; set; }
        public Guid CreatedByUser { get; set; }
        public Guid? UpdatedByUser { get; set; }
        public ActionTypeEnum ActionType { get; set; }
        [MaxLength(100)]
        public string EntityName { get; set; }
        public DateTime CreatedAt{ get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? OldValues { get; set; } // JSON representation of old values Note: incase Null its new value 
        public string? NewValues { get; set; } // JSON representation of new values Note incase null its deleted
    }
}
