using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.UpdateUser
{
    public record UpdateUserCommand(UserUpdateRequest UserUpdate) : IRequest<Result<AuthUserDetailsDTO>>;
}
