using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.ICartItemServices
{
    public interface ICartItemService
    {
        public Task<Result<bool>> CreateCartItem(CreateCartItemRequest request);
        public   Task<Result<bool>> AddCartItemList(IEnumerable<CreateCartItemRequest> request);
        public Task<Result<CartItemResponse>> UpdateCartItemQuantity(UpdateCartItemRequest request);
        public Task<Result<bool>> DeleteCartItem(Guid ItemID);
    }
}
