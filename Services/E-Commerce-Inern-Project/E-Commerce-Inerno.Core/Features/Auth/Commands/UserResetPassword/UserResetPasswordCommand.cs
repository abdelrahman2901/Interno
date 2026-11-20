using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Auth.Commands.UserResetPassword
{
    public record UserResetPasswordCommand(Guid UserID, string NewPassword) : IRequest<Result<bool>>;


}
