using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Query.GetAllWishListsQ
{
    public record GetAllUserWishListQuery(Guid UserID) : IRequest<Result<IEnumerable<WishListDetailsResponse>>>;
}
