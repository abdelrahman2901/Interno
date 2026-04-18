using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.CreateWishListCmd
{
    public record CreateWishListRequest(Guid UserID,Guid ProductID
      ) : IRequest<Result<WishListResponse>>;
}
