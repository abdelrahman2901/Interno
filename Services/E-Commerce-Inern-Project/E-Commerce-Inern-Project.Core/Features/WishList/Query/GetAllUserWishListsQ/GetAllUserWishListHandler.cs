
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Query.GetAllWishListsQ
{
    public class GetAllUserWishListHandler : IRequestHandler<GetAllUserWishListQuery, Result<IEnumerable<WishListDetailsResponse>>>
    {
      private readonly IWishListService _wishListService;
        public GetAllUserWishListHandler(IWishListService wishListService )
        {
            _wishListService = wishListService;
        }

        public async Task<Result<IEnumerable<WishListDetailsResponse>>> Handle(GetAllUserWishListQuery request, CancellationToken cancellationToken)
        {
            return await _wishListService.GetAllUserWishList(request.UserID);
        }
    }
}
