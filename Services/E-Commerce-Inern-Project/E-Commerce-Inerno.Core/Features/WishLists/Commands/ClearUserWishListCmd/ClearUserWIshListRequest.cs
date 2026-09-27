using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.WishLists.Commands.ClearUserWishListCmd
{
    public record ClearUserWIshListRequest(Guid UserID) : IRequest<Result<bool>>;
}
