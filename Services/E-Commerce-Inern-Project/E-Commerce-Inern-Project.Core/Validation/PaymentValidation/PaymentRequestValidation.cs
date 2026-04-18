using E_Commerce_Inern_Project.Core.Features.Payments.Command.CreateNewPaymentCmd;
using FluentValidation;


namespace E_Commerce_Inern_Project.Core.Validation.PaymentValidation
{
    public class PaymentRequestValidation :AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidation() { 
        RuleFor(x => x.UserID).NotEmpty().WithMessage("UserID is required.");
            RuleFor(x => x.PaymentMethod).IsInEnum().WithMessage("Invalid payment method.");    
        }
    }
}
