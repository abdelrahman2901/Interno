

using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Payment.Command.InitializePaymentCmd
{
    public class InitializePaymentCmd : IRequest<Result<string>>
    {
        public Guid UserID { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public InitializePaymentCmd(Guid userID, PaymentMethods paymentMethod)
        {
            UserID = userID;
            PaymentMethod = paymentMethod;
        }
    }
}
