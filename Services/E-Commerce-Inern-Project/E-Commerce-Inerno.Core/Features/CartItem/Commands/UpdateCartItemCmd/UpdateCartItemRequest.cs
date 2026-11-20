using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartItemDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.CartItem.Commands.UpdateCartItemCmd
{
    public record UpdateCartItemRequest(Guid CartItemID ,bool IsIncrease) : IRequest<Result<CartItemResponse>>;

}
