using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CartDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Cart.Query.GetUserCartDetailsQ
{
    public record GetUserCarItemsQuery(Guid UserID) : IRequest<Result<CartDetailsResponse>>;
}
