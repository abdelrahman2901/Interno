namespace E_Commerce_Interno.Domain.DTO.CartItemDTO
{
    public class CartItemResponse
    {
        public Guid CartItemID { get; set; }
        public Guid CartID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
