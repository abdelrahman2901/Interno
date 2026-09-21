using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ISizeRepo
{
    public interface ISizeRepository
    {
        public Task<Size?> CreateSize(Size size);
 
        public Task<IEnumerable<Size>> GetAllSizes();
        public Task<Size?> GetSize(Guid SizeID);
        public Task<bool> SaveChanges();
    }
}
