using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class OrderItems
    {
        [Key]
        [MaxLength(100)]
        public Guid OrderItemID { get; set; }

        [MaxLength(100)]
        public Guid OrderID {  get; set; }
        [ForeignKey(nameof(OrderID))]
        public Order Order { get; set; }

        [MaxLength(100)]
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public DateTime AddedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
