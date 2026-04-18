using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.UpdateCartItemCmd
{
    public record UpdateCartItemRequest(Guid CartItemID ,bool IsIncrease) : IRequest<Result<CartItemResponse>>;

}
