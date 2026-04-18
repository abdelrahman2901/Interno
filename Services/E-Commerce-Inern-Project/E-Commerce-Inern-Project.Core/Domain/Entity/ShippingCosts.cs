using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class ShippingCosts
    {
        [Key]
        [MaxLength(100)]
        public Guid ShippingCostID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [MaxLength(10)]
        public decimal ShippingCost { get; set; } = decimal.Zero;
        [MaxLength(100)]
        public Guid AraeID { get; set; }
        [ForeignKey(nameof(AraeID))]
        public Area Area { get; set; }
        public bool IsDeleted{ get; set; }
        
        public ICollection<Order >orders { get; set; }
    }
}
