using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Category()
    {
        [Key]
        [MaxLength(100)]
        public Guid CategoryID { get; set; }
        [MaxLength(50)]
        public string CategoryName { get; set; }
        [MaxLength(100)]
        public Guid? ParentCategoryID { get; set; }
        public Category? ParentCategory { get; set; }
        [MaxLength(200)]
        public string? CategoryImageUrl { get; set; }
        [MaxLength(200)]
        public string? HashImage { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
