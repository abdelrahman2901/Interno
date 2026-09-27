using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Commands.DeleteWishListCmd
{
    public record DeleteWishListRequest(Guid WishList) : IRequest<Result<bool>>;
}
