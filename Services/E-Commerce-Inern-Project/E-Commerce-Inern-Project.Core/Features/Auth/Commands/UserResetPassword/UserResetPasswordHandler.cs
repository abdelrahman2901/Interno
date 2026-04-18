using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.UserResetPassword
{
    public class UserResetPasswordHandler : IRequestHandler<UserResetPasswordCommand, Result<bool>>
    {
        private readonly IAuthService _AuthService;
        public UserResetPasswordHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<bool>> Handle(UserResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.UserResetPassword(request.UserID,request.NewPassword);
        }
    }
}
