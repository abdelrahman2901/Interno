using E_Commerce_Interno.Domain.DTO.PaymentDTO;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo
{
    public interface IPaymobService
    {
        public  Task<string?> InitializePayment(
          InitializePaymentDto initializePaymentDto,
           CancellationToken cancellationToken = default);
    }
        
}
