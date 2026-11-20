using E_Commerce_Interno.Domain.Common;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Auth.Commands.SignOutUser
{
    public record SignOutUserCommand(Guid UserID) : IRequest<Result<bool>>;
    
}
