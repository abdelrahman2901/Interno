using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.DeleteWishListCmd
{
    public class DeleteWishListHandler : IRequestHandler<DeleteWishListRequest, Result<bool>>
    {
        private readonly IWishListService _wishListService;
        public DeleteWishListHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<Result<bool>> Handle(DeleteWishListRequest request, CancellationToken cancellationToken)
        {
            return await _wishListService.DeleteWishList(request.WishList);
        }

    }
}
