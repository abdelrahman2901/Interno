using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Commands.CreateWishListCmd
{
    public record CreateWishListRequest(Guid UserID, Guid ProductID) : IRequest<Result<bool>>;
}
