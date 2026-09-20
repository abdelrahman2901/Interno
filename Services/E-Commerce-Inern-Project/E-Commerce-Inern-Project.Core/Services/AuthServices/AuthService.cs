using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRolesRepo;
using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.DTO.TokenDTO;
using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Enums;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;
using E_Commerce_Inern_Project.Core.Policies;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IJWTServices;
using Polly;
using System.Security.Claims;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _AuthRepo;
        private readonly IUserRepository _UserRepo;
        private readonly IJwtService _JwtService;
        private readonly ITransectionRepository _Transection;
        private readonly ICartRepository _CartRepo;
        private readonly IRabbitMQPublisher _Publish;
        private readonly IMapper _Mapper;
        //private readonly IPollyPolicy _asyncPolicy;
        private readonly IAsyncPolicy _policy;
        private readonly IUserRolesRepository _UserRolesRepo;
        private readonly string AuditRoutingKey = "Interno.Audit";

        public AuthService(IAuthRepository AuthRepo, IPollyPolicy asyncPolicy, IRabbitMQPublisher Publish, IUserRepository UserRepo, IUserRolesRepository UserRolesRepo, ICartRepository CartRepo, ITransectionRepository Transection, IJwtService JwtService, IMapper Mapper)
        {
            _AuthRepo = AuthRepo;
            _UserRepo = UserRepo;
            _UserRolesRepo = UserRolesRepo;
            _JwtService = JwtService;
            _CartRepo = CartRepo;
            _Transection = Transection;
            _Mapper = Mapper;
            //_asyncPolicy = asyncPolicy;
            _Publish = Publish;
            _policy = asyncPolicy.GetPolicy();
        }

        public async Task<Result<bool>> ChangeUserPassword(ChangeUserPasswordCommand Request)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByID(Request.UserID);
            if (user == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            if (!await _AuthRepo.IsPasswordCorrect(user.UserID, Request.CurrentPassword))
            {
                return Result<bool>.BadRequest("Incorrect current password");
            }

            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(user);


            user.Password = Request.NewPassword;

            if (!await _AuthRepo.SaveChanges())
            {
                return Result<bool>.BadRequest("Failed to change password");
            }
            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(user);
            AuditRequest AuditRequest = new(user.UserID, ActionTypeEnum.ChangePassword, nameof(ApplicationUser), jsonOldValues, jsonNewValues);
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);   
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, AuditRequest);
            });
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UserResetPassword(Guid UserID, string NewPassword)
        {
            ApplicationUser? User = await _UserRepo.GetApplicationUserByID(UserID);
            if (User == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(User);
            User.Password = NewPassword;

            if (!await _AuthRepo.SaveChanges())
            {
                return Result<bool>.BadRequest("Failed to change password");
            }

            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(User);

            AuditRequest Request = new(User.UserID, ActionTypeEnum.ResetPassword, nameof(ApplicationUser), jsonOldValues, jsonNewValues);

            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<bool>.Success(true);
        }

        public async Task<Result<AuthTokenResponse>> Login(LoginUserCommand Login)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByEmail(Login.Email);
            if (user == null)
            {
                return Result<AuthTokenResponse>.NotFound("User not found");
            }
            bool AccountValidation = await _AuthRepo.IsPasswordCorrect(user.UserID, Login.Password);
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
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<AuthTokenResponse>.Success(TokenDetails);
        }

        public async Task<Result<AuthTokenResponse>> RefreshToken(TokenModelDTO tokenModel)
        {
            try
            {
                Result<ClaimsPrincipal?> principal = _JwtService.GetPrincipalFromJwtToken(tokenModel.token);
                if (principal == null || !principal.IsSuccess)
                {
                    return Result<AuthTokenResponse>.BadRequest(principal.ErrorMessage);
                }

                ApplicationUser? user = await _UserRepo.GetApplicationUserByEmail(principal.Data?.FindFirst(ClaimTypes.Email).Value);

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

        public async Task<Result<AuthTokenResponse>> RegisterUser(RegisterUserCommand Register)
        {
            var transection = await _Transection.BeginTransactionAsync();
            if (transection == null)
            {

                return Result<AuthTokenResponse>.InternalError("Failed To Initial Transection");
            }
            try
            {


                if (await _UserRepo.IsEmailAlreadyRegister(Register.Email))
                {
                    return Result<AuthTokenResponse>.BadRequest("Email Already Registered");
                }

                ApplicationUser user = _Mapper.Map<ApplicationUser>(Register);
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
                //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
                //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
                //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
                //var policy = _asyncPolicy.GetPolicy();
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

        public async Task<Result<bool>> SignOut(Guid UserID)
        {
            var user = await _UserRepo.GetApplicationUserByID(UserID);
            if (user == null)
            {
                return Result<bool>.NotFound("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpirtation = null;

            if (!await _UserRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to sign out user");
            }

            AuditRequest Request = new(UserID, ActionTypeEnum.Logout, nameof(ApplicationUser), null, null);
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });
            return Result<bool>.Success(true);
        }

        public async Task<Result<AuthUserDetailsDTO?>> UpdateUser(UserUpdateRequest UpdateRequeset)
        {
            ApplicationUser? user = await _UserRepo.GetApplicationUserByID(UpdateRequeset.userID);
            if (user == null)
            {
                return Result<AuthUserDetailsDTO?>.NotFound("User not found");
            }

            string jsonOldValues = JsonSerializer.Serialize<ApplicationUser>(user);
            _Mapper.Map(UpdateRequeset, user);

            var Result = await _AuthRepo.SaveChanges();
            if (!Result)
            {
                return Result<AuthUserDetailsDTO?>.InternalError("Failed to update user");
            }

            AuthUserDetailsDTO UserDetails = _Mapper.Map<AuthUserDetailsDTO>(user);
            string jsonNewValues = JsonSerializer.Serialize<ApplicationUser>(user);
            AuditRequest Request = new(user.UserID, ActionTypeEnum.Update, nameof(ApplicationUser), jsonOldValues, jsonNewValues);
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<AuthUserDetailsDTO?>.Success(UserDetails);
        }

        public async Task<Result<bool>> DeleteUser(Guid UserID)
        {
            ApplicationUser? User = await _UserRepo.GetApplicationUserByID(UserID);
            if (User == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            User.IsDeleted = true;


            if (!await _AuthRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete user");
            }

            string JsonOldValues = JsonSerializer.Serialize<ApplicationUser>(User);
            AuditRequest Request = new(User.UserID, ActionTypeEnum.Delete, nameof(ApplicationUser), JsonOldValues, null);
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });
            

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> BlockUser(Guid UserID)
        {
            ApplicationUser? User = await _UserRepo.GetApplicationUserByID(UserID);
            if (User == null)
            {
                return Result<bool>.NotFound("User not found");
            }
            string JsonOldValues = JsonSerializer.Serialize<ApplicationUser>(User);
            User.IsBlocked = true;

            if (!await _AuthRepo.SaveChanges())
            {
                Result<bool>.InternalError("Failed to Block user");
            }

            string JsonNewValues = JsonSerializer.Serialize<ApplicationUser>(User);
            AuditRequest Request = new(User.UserID, ActionTypeEnum.Blocked, nameof(ApplicationUser), JsonOldValues, JsonNewValues);
            //var retryPolicy = _AsyncPolicy.GetRetryPolicy();
            //var circuitPolicy = _AsyncPolicy.GetCircuiteBreakerPolicy();
            //var policy =  Policy.WrapAsync(retryPolicy, circuitPolicy);
            //var policy = _asyncPolicy.GetPolicy();
            await _policy.ExecuteAsync(async () =>
            {
                await _Publish.Publish(AuditRoutingKey, Request);
            });

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> CheckPassword(Guid UserID, string Password)
        {
            bool result = await _AuthRepo.IsPasswordCorrect(UserID, Password);
            if (!result)
            {
                return Result<bool>.BadRequest("Wrong Password");
            }
            return Result<bool>.Success(result);
        }
    }
}
