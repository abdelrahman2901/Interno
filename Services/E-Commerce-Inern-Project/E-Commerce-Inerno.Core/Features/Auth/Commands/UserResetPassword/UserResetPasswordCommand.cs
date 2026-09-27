using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.UserResetPassword
{
    public record UserResetPasswordCommand(Guid UserID, string NewPassword) : IRequest<Result<bool>>;


}
