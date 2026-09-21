


using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICityRepo
{
    public interface ICityRepository
    {
        public Task<IEnumerable<City>> GetAllCities();
        public Task<City?> GetCity_Tracking(Guid id);
        public Task<City?> GetCity_NoTracking(Guid id);
        public Task<City?> AddCity(City City); 
        public Task<bool> IsCityExistsByName(string CityName);
        public   Task<bool> SaveChanges();
         

    }
}
