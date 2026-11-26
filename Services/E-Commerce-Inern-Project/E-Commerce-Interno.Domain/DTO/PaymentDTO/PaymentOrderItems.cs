namespace E_Commerce_Interno.Domain.DTO.PaymentDTO
{
    public class PaymentOrderItems
    {
        public string name { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public int quantity { get; set; }
    }
}
