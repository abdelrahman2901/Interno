using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.User.Query.GetAllUsers
{
    public record GetAllUsersQuery(int PageNumber = 1, int PageSize = 20) : IRequest<Result<PagedResult<AuthUserDetailsDTO>>>;
}
