namespace E_Commerce_Interno.Domain.DTO.CartDTO
{
    public class CartResponse
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public DateTime CreatedDate { get; set; }
       
    }
}
