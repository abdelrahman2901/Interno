
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo
{
    public interface IUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsers();
        public Task<ApplicationUser?> GetApplicationUserByID(Guid? userid, bool AsTracking);
        public Task<ApplicationUser?> GetApplicationUserByPhoneNumber(string Phone, bool AsTracking);
        public Task<ApplicationUser?> GetApplicationUserByEmail(string email, bool AsTracking);
        public Task<Guid> GetAdminID();
        public Task<bool> IsEmailAlreadyRegister(string Email);
        public Task<bool> SaveChanges();
    }
}
