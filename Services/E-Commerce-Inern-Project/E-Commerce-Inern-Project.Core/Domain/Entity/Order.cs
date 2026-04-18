using E_Commerce_Inern_Project.Core.Enums;
using E_Commerce_Inern_Project.Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class Order
    {
        [Key]
        [MaxLength(100)]
        public Guid OrderID { get; set; }
        
        [MaxLength(100)]
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }

        public Guid AddressID { get; set;  }
        [ForeignKey(nameof(AddressID))]
        public Address Address { get; set; }

        [MaxLength(100)]
        public Guid PaymentID { get; set; }
        [ForeignKey(nameof(PaymentID))]
        public Payments Payment { get; set; }
        [MaxLength(50)]
        public string OrderNumber { get; set; } // e.g., "ORD-2024-001"
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [MaxLength(50)]
        public decimal Subtotal { get; set; }=decimal.Zero;

        [MaxLength(100)]
        public Guid ShippingCostID { get; set; } 
        [ForeignKey(nameof(ShippingCostID))]
        public ShippingCosts ShippingCosts { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = decimal.Zero;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = decimal.Zero;

        [MaxLength(100)]
        public Guid? OrderCouponID { get; set; }
        [ForeignKey(nameof(OrderCouponID))]
        public OrderCoupons? OrderCoupon { get; set; }
         
         
        [MaxLength(50)]
        public OrderStatus OrderStatus { get; set; }  =OrderStatus.Processing;



        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
