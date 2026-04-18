using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.UpdateWishListCmd
{
    public record UpdateWishListRequest(Guid WishlistID, Guid UserID,
        Guid ProductID,
      DateTime AddedDate,
      bool IsDeleted) : IRequest<Result<WishListResponse>>;
}
