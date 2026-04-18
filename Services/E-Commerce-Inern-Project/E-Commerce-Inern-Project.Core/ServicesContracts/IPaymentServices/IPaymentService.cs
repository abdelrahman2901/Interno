using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IPaymentServices
{
    public interface IPaymentService 
    {
        public Task<Result<PaymentResponse>> AddNewPayment(PaymentRequest paymentRequest);
    }
}
 