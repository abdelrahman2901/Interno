using FluentValidation;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Commands.CreateWishListCmd
{
    public class CreateWishListREquestValidation : AbstractValidator<CreateWishListRequest>
    {
        public CreateWishListREquestValidation()
        {
            RuleFor(r => r.ProductID).NotEmpty().WithMessage(" Cant Be Emptyy");
            RuleFor(r => r.UserID).NotEmpty().WithMessage(" Cant Be Emptyy");
        }
    }
}
