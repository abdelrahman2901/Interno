using E_Commerce_Inern_Project.Core.Domain.ViewEntites;

namespace E_Commerce_Inern_Project.Core.DTO.WishListDTO
{
    public class WishListDetailsResponse
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public ProductDetails_vw Product { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
