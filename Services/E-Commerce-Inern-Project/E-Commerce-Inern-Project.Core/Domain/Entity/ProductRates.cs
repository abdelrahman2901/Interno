using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class ProductRates
    {
        [Key]
        [MaxLength(200)]
        public Guid RateID { get; set; }
        [MaxLength(200)]
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set;  }
        [MaxLength(200)]
        public Guid ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }
        [MaxLength(10)]
        public double Rating { get; set; }

        public bool IsDeleted { get; set; }
    }
}
