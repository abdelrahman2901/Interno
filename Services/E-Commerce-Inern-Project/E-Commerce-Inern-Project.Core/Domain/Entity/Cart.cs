using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Cart
    {
        [Key]
        [MaxLength(100)] 
        public Guid CartID { get; set; }
        [MaxLength(100)]
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public ICollection<CartItems> CartItems { get; set; }
        //public Order order { get; set;  }

    }
}
