using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IColorRepo
{
    public interface IColorRepository
    {
        public Task<IEnumerable<Colors>> GetAllColors();
        public Task<Colors?> GetColor(Guid ColorID);
        public Task<bool> CreateColors(Colors color);
        public Task<bool> SaveChanges();
    }
}
