using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAreaRepo
{
    public interface IAreaRepository
    {
        public Task<IEnumerable<Area>> GetAllAreas(); 
        public Task<Area?> GetArea_NoTracking(Guid id);
        public Task<Area?> GetArea_Tracking(Guid id); 
        public Task<bool> IsAreaExistsByName(string AreaName);
        public Task<bool> SaveChanges();
        public Task<bool> AddArea(Area area);

    }
}
