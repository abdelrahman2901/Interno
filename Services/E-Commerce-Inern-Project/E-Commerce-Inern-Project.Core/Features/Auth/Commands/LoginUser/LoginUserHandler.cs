using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using E_Commerce_Inern_Project.Core.Validation.UserValidation.LoginRequestValidation;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthService _AuthService;
        public LoginUserHandler(IAuthService authService , LoginRequestValidation validation)
        {
            _AuthService = authService;
            
        }

        public async Task<Result<AuthTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            
            return await _AuthService.Login(request);
        }
    }
}
