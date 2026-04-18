using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthService _AuthService;
        public RefreshTokenHandler(IAuthService authService)
        {
            _AuthService = authService;
        }

        public async Task<Result<AuthTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _AuthService.RefreshToken(request.TokenModel);
        }
    }
}
