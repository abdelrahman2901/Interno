using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo;
using E_Commerce_Inern_Project.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace E_Commerce_Inern_Project.Infrastructure.Repository.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<ApplicationRole> _RoleManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(UserManager<ApplicationUser> UserManager, RoleManager<ApplicationRole> RoleManager,
            SignInManager<ApplicationUser> SignInManager, ILogger<AuthRepository> logger)
        {
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _SignInManager = SignInManager;
            _logger = logger;
        }

        public async Task<bool> AccountValidation(string UserName, string Password)
        {
            try
            {
                var result = await _SignInManager.PasswordSignInAsync(UserName, Password, true, false);
                return result.Succeeded; 
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Validating Password  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Validating Password  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> SignIn(ApplicationUser User)
        {
            try
            { 
                await _SignInManager.SignInAsync(User, true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Loging in  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Loging in  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> SignOut()
        {
            try
            {
                await _SignInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Signing out User  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Signing out User  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> IsPasswordCorrect(ApplicationUser User, string Password)
        {
            try
            {
                return await _UserManager.CheckPasswordAsync(User, Password);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Checking if the password is correct Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Checking if the password is correct  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> ChangeUserPassword(ApplicationUser user, string oldPassword, string NewPassword)
        {
            try
            {
                var result = await _UserManager.ChangePasswordAsync(user, oldPassword, NewPassword);
                if (result.Errors != null && !result.Errors.Any())
                {
                    _logger.LogError("An error occurred while Changing User Password  Error : {message}", string.Join('-', result.Errors.Select(r => r.Description)));
                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Change User Password  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while  Change User Password  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> RegisterUser(ApplicationUser NewUser,string password)
        {
            try
            {
                var result = await _UserManager.CreateAsync(NewUser,password);

                if (result.Errors != null && result.Errors.Count() > 0)
                {
                    _logger.LogError("An error occurred while Registering New User  Error : {message}", string.Join('-', result.Errors.Select(r => r.Description)));
                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Registering New User  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Registering New User  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> UpdateUser(ApplicationUser UpdateUser)
        {
            try
            {
                var result = await _UserManager.UpdateAsync(UpdateUser);
                if (result != null)
                {
                    _logger.LogError("An error occurred while Updating User  Error : {message}", string.Join('-', result.Errors.Select(r => r.Description)));
                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Updating User  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Updating User  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> UserResetPassword(ApplicationUser UpdateUser, string NewPassword)
        {
            try
            {
             await   _UserManager.ResetPasswordAsync(UpdateUser, await _UserManager.GeneratePasswordResetTokenAsync(UpdateUser), NewPassword);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Reseting User Password  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Reseting User Password  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> AddUserToRole(ApplicationUser User, string Role)
        {
            try
            {
                var result= await _UserManager.AddToRoleAsync(User, Role);
                if (!result.Succeeded)
                {
                    _logger.LogError("An error occurred while Adding User To Role  Error : {message}", string.Join('-', result.Errors.Select(r => r.Description)));
                }
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Adding User To Role  Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Adding User To Role  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
