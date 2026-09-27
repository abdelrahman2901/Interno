using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CartDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Carts.Query.GetUserCartDetailsQ
{
    public record GetUserCarItemsQuery(Guid UserID) : IRequest<Result<CartDetailsResponse>>;
}
