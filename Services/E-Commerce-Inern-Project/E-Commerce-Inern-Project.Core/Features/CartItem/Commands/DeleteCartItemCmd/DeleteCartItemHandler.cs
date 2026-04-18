using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.DeleteCartItemCmd
{
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemRequest, Result<bool>>
    {
        private readonly ICartItemService _cartItemService;
        public DeleteCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<Result<bool>> Handle(DeleteCartItemRequest request, CancellationToken cancellationToken)
        {
            return await _cartItemService.DeleteCartItem(request.ItemID);
        }
    }
}
