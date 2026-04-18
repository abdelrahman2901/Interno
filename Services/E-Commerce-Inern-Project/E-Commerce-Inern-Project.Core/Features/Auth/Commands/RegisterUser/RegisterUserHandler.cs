using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;
using E_Commerce_Inern_Project.Core.Validation.UserValidation.RegisterRequestValidation;


namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthService _AuthService;
        public RegisterUserHandler(IAuthService authService , RegisterRequestValidation validation)
        {
            _AuthService = authService;
        }
        public async Task<Result<AuthTokenResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            
            return await _AuthService.RegisterUser(request);
        }
    }
}
