using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.PaymentDTO;
using E_Commerce_Inern_Project.Core.Enums;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd
{
    public record PaymentRequest(Guid UserID, decimal Amount, PaymentMethods PaymentMethod) : IRequest<Result<PaymentResponse>>;
  
}
