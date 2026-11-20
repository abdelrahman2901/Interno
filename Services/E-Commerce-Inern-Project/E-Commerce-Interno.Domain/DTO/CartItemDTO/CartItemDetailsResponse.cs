using E_Commerce_Interno.Domain.DTO.CartDTO;
using E_Commerce_Interno.Domain.ViewEntites;

namespace E_Commerce_Interno.Domain.DTO.CartItemDTO
{
    public class CartItemDetailsResponse
    {
        public Guid CartItemID { get; set; }
        public CartResponse Cart { get; set; }
        public ProductDetails_vw Product { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
