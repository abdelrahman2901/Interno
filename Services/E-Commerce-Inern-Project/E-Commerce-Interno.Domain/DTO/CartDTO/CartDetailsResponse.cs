using E_Commerce_Interno.Domain.DTO.CartItemDTO;

namespace E_Commerce_Interno.Domain.DTO.CartDTO
{
    public class CartDetailsResponse
    {
        public Guid CartID { get; set; } 
        public Guid UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<CartItemDetailsResponse> CartItems { get; set; }
    }
}
