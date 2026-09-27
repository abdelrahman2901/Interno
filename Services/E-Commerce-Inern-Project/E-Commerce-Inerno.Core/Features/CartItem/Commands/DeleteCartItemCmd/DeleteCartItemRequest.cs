using E_Commerce_Interno.Domain.Common;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.CartItem.Commands.DeleteCartItemCmd
{
    public record DeleteCartItemRequest(Guid ItemID) : IRequest<Result<bool>>;
    
}
