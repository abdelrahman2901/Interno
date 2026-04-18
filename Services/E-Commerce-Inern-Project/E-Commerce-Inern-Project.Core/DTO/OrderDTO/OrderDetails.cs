using E_Commerce_Inern_Project.Core.DTO.AddressDTO;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.DTO.OrderItemsDTO;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.Enums;


namespace E_Commerce_Inern_Project.Core.DTO.OrderDTO
{
    public class OrderDetails
    {
        public Guid OrderID { get; set; }
        public IEnumerable<OrderItemDetails> OrderItems { get; set; }
        public AddressResponse Address { get; set; }
        //public PaymentResponse Payment { get; set;
        public string PaymentMethod { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string OrderNumber { get; set; } // e.g., "ORD-2024-001"
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; } = decimal.Zero;
        public ShippingCostDetails ShippingCosts { get; set; }
        public decimal DiscountAmount { get; set; } = decimal.Zero;
        public decimal TotalAmount { get; set; } = decimal.Zero;
        public OrderCouponResponse? OrderCoupon { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
    }
}
