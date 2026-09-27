using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Polly;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IJwtService _JwtService;
        private readonly IRabbitMQPublisher _Publish;
        private readonly IAsyncPolicy _policy;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public LoginUserHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IRabbitMQPublisher Publish, IUserRepository UserRepo, IJwtService JwtService)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _JwtService = JwtService;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<AuthTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByEmail(request.Email, true);
            if (user == null)
            {
                return Result<AuthTokenResponse>.NotFound("User not found");
            }
            bool AccountValidation = await _AuthRepo.IsPasswordCorrect(user.UserID, request.Password);
            if (!AccountValidation)
            {
                return Result<AuthTokenResponse>.BadRequest("Incorrect password Or Email");
            }

            AuthTokenResponse TokenDetails = await _JwtService.CreateJwtToken(user);

            user.RefreshTokenExpirtation = TokenDetails.RefreshTokenExpirationDateTime;
            user.RefreshToken = TokenDetails.RefreshToken;
            if (!await _AuthRepo.SaveChanges())
            {
                return Result<AuthTokenResponse>.InternalError("Failed to update user with refresh token");
            }

            AuditRequest Request = new(user.UserID, ActionTypeEnum.Login, nameof(ApplicationUser), null, null);

            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<AuthTokenResponse>.Success(TokenDetails);
        }
    }
}
