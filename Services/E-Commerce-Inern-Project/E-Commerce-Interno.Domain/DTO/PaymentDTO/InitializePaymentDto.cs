using E_Commerce_Interno.Domain.Enums;

namespace E_Commerce_Interno.Domain.DTO.PaymentDTO
{
    public class InitializePaymentDto
    {
        public decimal TotalAmount { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid PaymentId { get; set; }
        public PaymentOrderItems[] Items { get; set; } = Array.Empty<PaymentOrderItems>();
        public string Currency { get; set; } = "EGP";
    }
}
