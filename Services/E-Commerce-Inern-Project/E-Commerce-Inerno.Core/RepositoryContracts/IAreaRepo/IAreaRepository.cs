using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IAreaRepo
{
    public interface IAreaRepository
    {
        public Task<IEnumerable<Area>> GetAllAreas(); 
        public Task<Area?> GetArea(Guid id,bool AsTracking);
        public Task<bool> IsAreaExistsByName(string AreaName);
        public Task<bool> SaveChanges();
        public Task<bool> AddArea(Area area);

    }
}
