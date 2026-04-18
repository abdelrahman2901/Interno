using E_Commerce_Inern_Project.Core.Identity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo
{
    public interface IAuthRepository
    {
        public Task<bool> RegisterUser(ApplicationUser Register, string Password);
        public Task<bool> SignIn(ApplicationUser user);
        public Task<bool> AccountValidation(string Email, string Password);
        public Task<bool> SignOut();
        public Task<bool> UpdateUser(ApplicationUser UpdateUser);
        public Task<bool> ChangeUserPassword(ApplicationUser user, string oldPassword, string NewPassword);
        public Task<bool> UserResetPassword(ApplicationUser UpdateUser, string NewPassword);
        public Task<bool> IsPasswordCorrect(ApplicationUser User, string Password);
        public Task<bool> AddUserToRole(ApplicationUser User, string Role);
    }
}
