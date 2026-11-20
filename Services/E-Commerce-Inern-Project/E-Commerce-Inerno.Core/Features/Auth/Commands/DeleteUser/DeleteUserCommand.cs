using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserID) : IRequest<Result<bool>>;
   
}
