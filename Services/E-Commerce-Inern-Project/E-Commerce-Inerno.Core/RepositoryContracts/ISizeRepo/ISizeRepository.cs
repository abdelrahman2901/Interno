
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.ISizeRepo
{
    public interface ISizeRepository
    {
        public Task<IEnumerable<Size>> GetAllSizes();
        public Task<Size?> GetSize(Guid SizeID);
        public Task<bool> CreateSize(Size size);
        public Task<bool> SaveChanges();
    }
}
