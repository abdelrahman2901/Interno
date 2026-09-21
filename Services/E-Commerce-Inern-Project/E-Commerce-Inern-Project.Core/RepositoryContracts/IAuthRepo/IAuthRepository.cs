using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAuthRepo
{
    public interface IAuthRepository
    {
        public Task<bool> RegisterUser(ApplicationUser Register);
        public Task<bool> SaveChanges();
        public Task<bool> IsPasswordCorrect(Guid UserID, string Password);
    }
}
