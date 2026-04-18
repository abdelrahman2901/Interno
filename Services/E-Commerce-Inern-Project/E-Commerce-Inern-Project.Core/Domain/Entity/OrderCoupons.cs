using E_Commerce_Inern_Project.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class OrderCoupons
    {
        [Key]
        [MaxLength(100)]
        public Guid OrderCouponID {  get; set; }
        [MaxLength(100)]
        public string CouponCode { get; set; }
        [MaxLength(10)]
        public decimal Discount { get; set; }
        [MaxLength(10)]
        public DiscountTypeEnum DiscountType { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Order> orders { get; set; }
    }
}
