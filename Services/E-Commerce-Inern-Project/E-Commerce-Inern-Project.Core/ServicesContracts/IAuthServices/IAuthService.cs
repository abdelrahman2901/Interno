using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AuthDTO;
using E_Commerce_Inern_Project.Core.DTO.TokenDTO;
using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.ChangeUserPassword;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.LoginUser;
using E_Commerce_Inern_Project.Core.Features.Auth.Commands.RegisterUser;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IAuthServices
{  
    public interface IAuthService
    {
        public Task<Result<AuthTokenResponse>> RegisterUser(RegisterUserCommand Register);
        public Task<Result<AuthTokenResponse>> Login(LoginUserCommand Login); 
        public Task<Result<bool>> SignOut(Guid UserID);
        public Task<Result<AuthUserDetailsDTO?>> UpdateUser(UserUpdateRequest UpdateRequeset);
        public Task<Result<bool>> DeleteUser(Guid UserID);
        public Task<Result<bool>> BlockUser(Guid UserID);
        public Task<Result<bool>> ChangeUserPassword(ChangeUserPasswordCommand Request);
        public Task<Result<bool>> UserResetPassword(Guid UserID, string NewPassword);
        public Task<Result<AuthTokenResponse>> RefreshToken(TokenModelDTO tokenModel);
    
        public Task<Result<bool>> CheckPassword(Guid UserID, string Password);

    }
}
