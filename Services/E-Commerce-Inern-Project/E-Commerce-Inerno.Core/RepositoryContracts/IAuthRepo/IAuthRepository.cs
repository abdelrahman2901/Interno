using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IAuthRepo
{
    public interface IAuthRepository
    {
        public Task<bool> RegisterUser(ApplicationUser Register);
        public Task<bool> SaveChanges();
        public Task<bool> IsPasswordCorrect(Guid UserID, string Password);
    }
}
