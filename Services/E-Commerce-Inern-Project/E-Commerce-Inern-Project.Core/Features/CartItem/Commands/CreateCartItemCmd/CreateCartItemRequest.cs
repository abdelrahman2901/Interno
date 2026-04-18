using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartItemDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd
{
    public record CreateCartItemRequest(Guid UserID,Guid ProductID) : IRequest<Result<bool>>;
}
