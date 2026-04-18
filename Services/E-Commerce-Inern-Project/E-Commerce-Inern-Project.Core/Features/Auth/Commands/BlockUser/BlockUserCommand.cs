using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.BlockUser
{
    public record BlockUserCommand(Guid UserID) : IRequest<Result<bool>>;
  
}
