using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.CreateWishListCmd
{
    public class CreateWishListHandler : IRequestHandler<CreateWishListRequest, Result<WishListResponse>>
    {
        private readonly IWishListService _wishListService;
        public CreateWishListHandler(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        public async Task<Result<WishListResponse>> Handle(CreateWishListRequest request, CancellationToken cancellationToken)
        {
            return await _wishListService.CreateWishList(request);
        }
    }
}
