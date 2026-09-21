using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Size
    {
        [Key]
        [MaxLength(100)]
        public Guid SizeID { get; set; }
        [MaxLength(10)]
        public string SizeName { get; set; }
        public bool IsDeleted { get; set; } =false;
        public ICollection<Product> Products { get; set; }
    }
}
