using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.SignOutUser
{
    public record SignOutUserCommand(Guid UserID) : IRequest<Result<bool>>;
    
}
