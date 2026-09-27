using E_Commerce_Inern_Project.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using System.Security.Claims;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IJwtService _JwtService;

        public RefreshTokenHandler(IAuthRepository AuthRepo, IUserRepository UserRepo, IJwtService JwtService)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _JwtService = JwtService;
        }

        public async Task<Result<AuthTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenModel = request.TokenModel;
            try
            {
                Result<ClaimsPrincipal?> principal = _JwtService.GetPrincipalFromJwtToken(tokenModel.token);
                if (principal == null || !principal.IsSuccess)
                {
                    return Result<AuthTokenResponse>.BadRequest(principal.ErrorMessage);
                }

                ApplicationUser? user = await _UserRepo.GetApplicationUserByEmail(principal.Data?.FindFirst(ClaimTypes.Email).Value, true);

                if (user == null || user.RefreshToken != tokenModel.refreshToken || user.RefreshTokenExpirtation <= DateTime.UtcNow)
                {
                    return Result<AuthTokenResponse>.BadRequest($@"Invalid refresh token , possible Reasons : 
                            user:{user.PersonName}, user token doesnt equel to tokenmodel :{user.RefreshToken != tokenModel.refreshToken} ,
                         user refresh token is expired : {user.RefreshTokenExpirtation <= DateTime.UtcNow}");
                }

                AuthTokenResponse authenticationResponse = await _JwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpirtation = authenticationResponse.RefreshTokenExpirationDateTime;

                return await _AuthRepo.SaveChanges() ? Result<AuthTokenResponse>.Success(authenticationResponse)
                     : Result<AuthTokenResponse>.InternalError("Failed To Update User After Refresh Token");
            }
            catch (Exception ex)
            {
                return Result<AuthTokenResponse>.InternalError(ex.Message);
            }
        }
    }
}
