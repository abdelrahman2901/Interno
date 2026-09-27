using E_Commerce_Interno.Domain.ViewEntites;

namespace E_Commerce_Interno.Domain.DTO.OrderItemsDTO
{
    public class OrderItemDetails
    {
        public Guid OrderItemID { get; set; }
        public Guid OrderID { get; set; }
        public ProductDetails_vw Product { get; set; } 
        public DateTime AddedDate { get; set; }
        public int Quantity { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
