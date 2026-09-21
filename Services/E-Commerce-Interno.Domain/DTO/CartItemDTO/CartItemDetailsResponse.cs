using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;


namespace E_Commerce_Inern_Project.Core.DTO.CartItemDTO
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
