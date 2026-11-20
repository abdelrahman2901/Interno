using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.ICityRepo
{
    public interface ICityRepository
    {
        public Task<IEnumerable<City>> GetAllCities();
        public Task<City?> GetCity(Guid id, bool AsTracking);
        public Task<bool> AddCity(City City);
        public Task<bool> IsCityExistsByName(string CityName);
        public Task<bool> SaveChanges();


    }
}
