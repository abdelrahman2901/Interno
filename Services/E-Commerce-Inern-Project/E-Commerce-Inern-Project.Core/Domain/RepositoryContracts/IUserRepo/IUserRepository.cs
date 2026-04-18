using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Identity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo
{
    public interface IUserRepository
    {
         
        public Task<IEnumerable<AuthUserDetailsDTO>> GetAllUsers();
        public Task<string?> GetUserRole(ApplicationUser user);
        public Task<ApplicationUser?> GetApplicationUserByID(Guid? userid);
        public Task<ApplicationUser?> GetApplicationUserByEmail(string email);
        public Task<bool> IsEmailAlreadyRegister(string Email);
 
    }
}
