using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartCmd
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemRequest, Result<CartItemResponse>>
    {
        private readonly ICartItemService _CartItemService;
        public UpdateCartItemHandler(ICartItemService CartItemService)
        {
            _CartItemService = CartItemService;
        }

        public async Task<Result<CartItemResponse>> Handle(UpdateCartItemRequest request, CancellationToken cancellationToken)
        {
            return await _CartItemService.UpdateCartItemQuantity(request);
        }
    }
}
