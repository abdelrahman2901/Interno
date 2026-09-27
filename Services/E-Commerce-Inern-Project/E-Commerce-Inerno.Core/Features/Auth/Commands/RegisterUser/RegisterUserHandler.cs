using AutoMapper;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRolesRepo;
using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AuthDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using MediatR;
using Polly;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<AuthTokenResponse>>
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IJwtService _JwtService;
        private readonly ITransectionRepository _Transection;
        private readonly ICartRepository _CartRepo;
        private readonly IRabbitMQPublisher _Publish;
        private readonly IMapper _Mapper;
        private readonly IAsyncPolicy _policy;
        private readonly IUserRolesRepository _UserRolesRepo;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public RegisterUserHandler(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IRabbitMQPublisher Publish, IUserRepository UserRepo, IUserRolesRepository UserRolesRepo, ICartRepository CartRepo, ITransectionRepository Transection, IJwtService JwtService, IMapper Mapper)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _UserRolesRepo = UserRolesRepo;
            _JwtService = JwtService;
            _CartRepo = CartRepo;
            _Transection = Transection;
            _Mapper = Mapper;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<AuthTokenResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var transection = await _Transection.BeginTransactionAsync();
            if (transection == null)
            {
                return Result<AuthTokenResponse>.InternalError("Failed To Initial Transection");
            }
            try
            {
                if (await _UserRepo.IsEmailAlreadyRegister(request.Email))
                {
                    return Result<AuthTokenResponse>.BadRequest("Email Already Registered");
                }

                ApplicationUser user = _Mapper.Map<ApplicationUser>(request);
                var Registerresult = await _AuthRepo.RegisterUser(user);
                if (!Registerresult)
                {
                    return Result<AuthTokenResponse>.InternalError("Failed to register user");
                }

                if (!await _UserRolesRepo.AddUserToRole(user.UserID, RolesEnum.Customer))
                {
                    return Result<AuthTokenResponse>.InternalError("Failed to assign role to user after registration");
                }

                var token = await _JwtService.CreateJwtToken(user);
                user.RefreshTokenExpirtation = token.RefreshTokenExpirationDateTime;
                user.RefreshToken = token.RefreshToken;

                if (!await _AuthRepo.SaveChanges())
                {
                    return Result<AuthTokenResponse>.InternalError("Failed to update user with refresh token after registration");
                }
                var CartREsult = await _CartRepo.CreateCart(new() { CartID = Guid.NewGuid(), UserID = user.UserID, CreatedDate = DateTime.Now });
                if (CartREsult == null)
                {
                    return Result<AuthTokenResponse>.InternalError("Failed To Create User Cart");
                }
                if (!await _CartRepo.SaveChanges())
                {
                    return Result<AuthTokenResponse>.InternalError("Failed To Save Changes For User Cart");
                }

                string JsonNewValues = JsonSerializer.Serialize<ApplicationUser>(user);
                AuditRequest Request = new(user.UserID, ActionTypeEnum.Register, nameof(ApplicationUser), null, JsonNewValues);

                await _policy.ExecuteAsync(async () =>
                {
                    await _Publish.Publish(AuditRoutingKey, Request);
                });

                await transection.CommitAsync();
                return Result<AuthTokenResponse>.Success(token);
            }
            catch (Exception ex)
            {
                await transection.RollbackAsync();

                return Result<AuthTokenResponse>.InternalError(ex.Message);
            }
        }
    }
}
