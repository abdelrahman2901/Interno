using E_Commerce_Interno.Domain.Enums;

namespace E_Commerce_Interno.Domain.DTO.PaymentDTO
{
    public class PaymentResponse
    {
        public Guid PaymentID { get; set; }
        public Guid UserID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
