using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.User.Query.GetUserDetailsByID
{
    public record GetUserDetailsByIDQuery(Guid UserID) : IRequest<Result<AuthUserDetailsDTO>>;
}
