using E_Commerce_Inerno.Core.RepositoryContracts.IWishListRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.WishLists.Commands.ClearUserWishListCmd
{
    public class ClearUserWishListHandler : IRequestHandler<ClearUserWIshListRequest, Result<bool>>
    {
        private readonly IWishListRepository _WishListRepo;

        public ClearUserWishListHandler(IWishListRepository WishListRepo)
        {
            _WishListRepo = WishListRepo;
        }

        public async Task<Result<bool>> Handle(ClearUserWIshListRequest request, CancellationToken cancellationToken)
        {
            var WIshLists = await _WishListRepo.GetAllUserWishList(request.UserID);

            foreach (WishList item in WIshLists)
            {
                item.IsDeleted = true;
            }
            if (!await _WishListRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To SaveChanges");
            }

            return Result<bool>.Success(true);
        }
    }
}
