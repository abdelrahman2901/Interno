namespace E_Commerce_Inern_Project.Core.DTO.OrderItemsDTO
{
    public class UpdateOrderItemRequest
    {
        public Guid OrderItemID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public DateTime AddedDate { get; set; }
        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }
    }
}
