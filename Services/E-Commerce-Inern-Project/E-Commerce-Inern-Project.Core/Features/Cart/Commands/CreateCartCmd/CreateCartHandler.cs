using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartServices;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.Cart.Commands.CreateCartCmd
{
    internal class CreateCartHandler : IRequestHandler<CreateCartRequest, Result<CartResponse>>
    {
        private readonly ICartService _cartService;
        public CreateCartHandler(ICartService cartService)
        {
            _cartService= cartService;
        }

        public async Task<Result<CartResponse>> Handle(CreateCartRequest request, CancellationToken cancellationToken)
        {
            return await _cartService.CreateCart(request.UserID);
        }
    }
}
