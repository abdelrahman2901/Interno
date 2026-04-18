using E_Commerce_Inern_Project.Core.Enums;
using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Payments
    {
        [Key]
        [MaxLength(50)]
        public Guid PaymentID { get; set; }
        [MaxLength(50)]
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }
        [MaxLength(100)]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [MaxLength(100)]
        public PaymentMethods PaymentMethod { get; set; }
        [MaxLength(100)]
        public StatusEnum Status { get; set; }
        public bool IsDeleted { get; set; }=false;
    }
}
