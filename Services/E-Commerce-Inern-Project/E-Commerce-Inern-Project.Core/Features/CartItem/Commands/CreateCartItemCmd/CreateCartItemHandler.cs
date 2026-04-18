using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd
{
    public class CreateCartItemHandler : IRequestHandler<CreateCartItemRequest, Result<bool>>
    {
        private readonly ICartItemService _cartItemService;
        public CreateCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<Result<bool>> Handle(CreateCartItemRequest request, CancellationToken cancellationToken)
        {
            return await _cartItemService.CreateCartItem(request);
        }
    }
}
