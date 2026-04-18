using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.CartItem.Commands.CreateCartItemCmd;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.AddCartItemListCmd
{
    public record AddCartItemListRequest(IEnumerable<CreateCartItemRequest> request) : IRequest<Result<bool>>;
}
