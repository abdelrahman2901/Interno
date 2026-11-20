using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.WishListDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.WishLists.Query.GetAllWishListsQ
{
    public record GetAllUserWishListQuery(Guid UserID) : IRequest<Result<IEnumerable<WishListDetailsResponse>>>;
}
