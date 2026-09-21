using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Product()
    {
        [Key]
        [MaxLength(100)]
        public Guid ProductID { get; set; }
        [MaxLength(100)]
        public Guid CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; }
        [MaxLength(10)]
        public decimal Price { get; set; }
        [MaxLength(10)]
        public decimal? SalePrice { get; set; } = 0;
        [MaxLength(10)]
        public int Stock { get; set; }
        [MaxLength(10)]
        public double Rating { get; set; } = 0;
        [MaxLength(10)]
        public int TotalRating { get; set; } = 0;

        [MaxLength(100)]
        public Guid SizeID { get; set; }
        [ForeignKey(nameof(SizeID))]
        public Size Size { get; set; }
        
        [MaxLength(100)]
        public Guid ColorID { get; set; }
        [ForeignKey(nameof(ColorID))]
        public Colors Color { get; set; }
        
        public DateTime CreatedAt { get; set; }
        [MaxLength(200)]
        public string ProductImageUrl { get; set; }
        [MaxLength(100)]
        public string HashImage { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public ICollection<ProductRates> ProductRates { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
        public ICollection<WishList> WishLists { get; set; }

    }
}