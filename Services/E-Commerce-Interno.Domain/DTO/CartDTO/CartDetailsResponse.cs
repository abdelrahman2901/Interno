using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;

namespace E_Commerce_Inern_Project.Core.DTO.CartDTO
{
    public class CartDetailsResponse
    {
        public Guid CartID { get; set; } 
        public Guid UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<CartItemDetailsResponse> CartItems { get; set; }
    }
}
