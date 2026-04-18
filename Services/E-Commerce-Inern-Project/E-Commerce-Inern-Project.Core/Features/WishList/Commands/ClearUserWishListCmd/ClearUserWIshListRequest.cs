using E_Commerce_Inern_Project.Core.Common;
using MediatR;
namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.ClearUserWishListCmd
{
    public record ClearUserWIshListRequest(Guid UserID) : IRequest<Result<bool>>;
}