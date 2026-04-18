using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Common;
 

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IUserServices
{
    public interface IUserService
    {
        public Task<Result<IEnumerable<AuthUserDetailsDTO>>> GetAllUsers();
        public Task<Result<AuthUserDetailsDTO>> GetUserDetailsByID(Guid userid);
        public Task<Result<AuthUserDetailsDTO>> GetUserDetailsByEmail(string email);
        //public Task<Result<ApplicationUser>> GetApplicationUserByID(Guid userid);
        //public Task<Result<ApplicationUser>> GetApplicationUserByEmail(string email);
        //public Task<Result<bool>> IsAccountDeleted(string Email);
        //public Task<Result<bool>> IsEmailAlreadyRegister(string Email);
         
    }
}
