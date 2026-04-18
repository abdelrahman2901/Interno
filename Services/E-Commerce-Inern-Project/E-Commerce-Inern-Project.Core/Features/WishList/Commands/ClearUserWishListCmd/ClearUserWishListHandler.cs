using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.ClearUserWishListCmd
{
    public class ClearUserWishListHandler : IRequestHandler<ClearUserWIshListRequest, Result<bool>>
    {
        private readonly IWishListService _wishListService;
        public ClearUserWishListHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<Result<bool>> Handle(ClearUserWIshListRequest request, CancellationToken cancellationToken)
        {
            return await _wishListService.ClearWishList(request.UserID);
        }
    }
}
