using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo
{
    public interface IUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsers();
        public Task<ApplicationUser?> GetApplicationUserByID(Guid? userid);
        public Task<ApplicationUser?> GetApplicationUserByPhoneNumber(string Phone);
        public Task<ApplicationUser?> GetApplicationUserByEmail(string email);
        public Task<bool> IsEmailAlreadyRegister(string Email);
        public Task<bool> SaveChanges();
    }
}
