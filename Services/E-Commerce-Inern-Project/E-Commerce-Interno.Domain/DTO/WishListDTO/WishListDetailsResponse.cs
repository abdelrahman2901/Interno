using E_Commerce_Interno.Domain.ViewEntites;

namespace E_Commerce_Interno.Domain.DTO.WishListDTO
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
