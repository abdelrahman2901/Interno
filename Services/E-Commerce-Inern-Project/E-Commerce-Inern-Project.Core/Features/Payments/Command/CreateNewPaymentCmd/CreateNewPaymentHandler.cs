using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IPaymentServices;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd
{
    public class CreateNewPaymentHandler : IRequestHandler<PaymentRequest, Result<PaymentResponse>>
    {
        private readonly IPaymentService _paymentService;
        public CreateNewPaymentHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<Result<PaymentResponse>> Handle(PaymentRequest request, CancellationToken cancellationToken)
        {
            return await _paymentService.AddNewPayment(request);
        }
    }
}
