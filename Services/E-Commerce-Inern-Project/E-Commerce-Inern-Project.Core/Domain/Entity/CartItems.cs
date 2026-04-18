using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class CartItems
    {
        [Key]
        [MaxLength(100)]
        public Guid CartItemID { get; set; }
        [MaxLength(100)] 
        public Guid CartID { get; set; }
        [ForeignKey(nameof(CartID))]
        public Cart Cart { get; set; }
        [MaxLength(100)]
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; } =false;
    }
}
