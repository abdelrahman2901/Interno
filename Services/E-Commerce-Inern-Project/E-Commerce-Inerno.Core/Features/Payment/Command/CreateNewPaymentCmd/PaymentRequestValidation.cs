using FluentValidation;

namespace E_Commerce_Inerno.Core.Features.Payment.Command.CreateNewPaymentCmd
{
    public class PaymentRequestValidation : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidation()
        {
            RuleFor(x => x.UserID).NotEmpty().WithMessage("UserID is required.");
            RuleFor(x => x.PaymentMethod).IsInEnum().WithMessage("Invalid payment method.");
        }
    }
}
