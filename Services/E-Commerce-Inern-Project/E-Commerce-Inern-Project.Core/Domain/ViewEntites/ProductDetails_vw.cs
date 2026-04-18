using E_Commerce_Inern_Project.Core.DTO.CategoryDTO;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.ViewEntites
{
    public class ProductDetails_vw
    {
        public Guid ProductID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid ParentcategoryID { get; set; }
        public Guid SizeID { get; set; }
        public Guid? ColorID { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }
        public int TotalRating { get; set; } 
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductImageUrl { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted{ get; set; }
 
    }
}
