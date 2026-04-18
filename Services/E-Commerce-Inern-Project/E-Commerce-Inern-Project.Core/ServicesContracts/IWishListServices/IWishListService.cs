using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.WishListDTO;
using E_Commerce_Inern_Project.Core.Features.WishList.Commands.CreateWishListCmd;
using E_Commerce_Inern_Project.Core.Features.WishList.Commands.UpdateWishListCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IWishListServices
{
    public interface IWishListService
    {
        public Task<Result<IEnumerable<WishListDetailsResponse>>> GetAllUserWishList(Guid UserID);
        public Task<Result<WishListResponse>> CreateWishList(CreateWishListRequest request);
        public Task<Result<WishListResponse>> UpdateWishList(UpdateWishListRequest request);
        public Task<Result<bool>> DeleteWishList(Guid WishList);
        public Task<Result<bool>> ClearWishList(Guid UserID);


    }
}