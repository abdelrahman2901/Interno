using MediatR;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.UserDTO;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID
{
    public record GetUserDetailsByIDQuery(Guid UserID) : IRequest<Result<AuthUserDetailsDTO>>;
}
