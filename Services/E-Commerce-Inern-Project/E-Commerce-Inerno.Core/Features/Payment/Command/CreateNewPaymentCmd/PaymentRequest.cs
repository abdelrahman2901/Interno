using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.PaymentDTO;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Payment.Command.CreateNewPaymentCmd
{
    public record PaymentRequest(Guid UserID, decimal Amount, PaymentMethods PaymentMethod) : IRequest<Result<PaymentResponse>>;
}
