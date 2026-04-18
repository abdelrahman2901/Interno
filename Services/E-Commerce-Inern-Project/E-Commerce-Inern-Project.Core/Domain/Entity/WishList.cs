using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class WishList
    {
        [Key]
        [MaxLength(100)]
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
