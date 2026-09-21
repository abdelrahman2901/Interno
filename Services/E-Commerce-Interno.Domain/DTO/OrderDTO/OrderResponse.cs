using E_Commerce_Inern_Project.Core.Enums;

namespace E_Commerce_Inern_Project.Core.DTO.OrderDTO
{
    public class OrderResponse
    {
        public Guid OrderID { get; set; }
        public Guid AddressID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid OrderCouponID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public Guid ShippingCostsID { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
