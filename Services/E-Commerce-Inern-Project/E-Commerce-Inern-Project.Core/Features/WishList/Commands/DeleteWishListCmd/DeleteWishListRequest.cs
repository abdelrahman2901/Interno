using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishList.Commands.DeleteWishListCmd
{
    public record DeleteWishListRequest(Guid WishList) : IRequest<Result<bool>>;
}
