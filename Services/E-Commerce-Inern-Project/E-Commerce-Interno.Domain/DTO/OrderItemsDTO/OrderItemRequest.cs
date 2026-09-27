namespace E_Commerce_Interno.Domain.DTO.OrderItemsDTO
{
    public class OrderItemRequest
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; } 
        public int Quantity { get; set; }

    }
}
