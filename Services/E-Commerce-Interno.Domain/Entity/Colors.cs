using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Colors
    {
        [Key]
        public Guid ColorID { get; set; }
        public string ColorName { get; set; }
        public string ColorHexCode { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Product> Products { get; set; }
        public ICollection<BannerSlide> BannerSlides { get; set; }
    
    }
}
