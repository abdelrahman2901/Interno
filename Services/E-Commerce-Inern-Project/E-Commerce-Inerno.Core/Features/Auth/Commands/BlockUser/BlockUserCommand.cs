using E_Commerce_Interno.Domain.Common;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Auth.Commands.BlockUser
{
    public record BlockUserCommand(Guid UserID) : IRequest<Result<bool>>;
  
}
