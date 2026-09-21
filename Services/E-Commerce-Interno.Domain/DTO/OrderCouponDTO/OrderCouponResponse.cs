using E_Commerce_Inern_Project.Core.Enums;

namespace E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO
{
    public class OrderCouponResponse
    {
        public Guid OrderCouponID { get; set; }
        public string CouponCode { get; set; }
        public decimal Discount { get; set; }
        public DiscountTypeEnum DiscountType { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
