using E_Commerce_Inerno.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.CityRepo
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<CityRepository> _logger;
        public CityRepository(ApplicationDBContext context, ILogger<CityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddCity(City City)
        {
            try
            {
                await _context.City.AddAsync(City);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While AddCity Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While AddCity InnerException: {message}", ex.InnerException.Message);

                }
                return false;
            }
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            try
            {
                return await _context.City.AsNoTracking().Where(r => !r.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllCities Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetAllCities InnerException: {message}", ex.InnerException.Message);

                }
                return [];
            }
        }

        public async Task<City?> GetCity(Guid id, bool AsTracking)
        {
            try
            {
                IQueryable<City> query = AsTracking ? _context.City : _context.City.AsNoTracking();
                return await query.FirstOrDefaultAsync(c => c.CityID == id && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCity Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetCity InnerException: {message}", ex.InnerException.Message);

                }
                return null;
            }
        }

        public async Task<bool> IsCityExistsByName(string CityName)
        {
            try
            {
                return await _context.City.AnyAsync(c => c.CityName == CityName && !c.IsDeleted);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While  Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While  InnerException: {message}", ex.InnerException.Message);

                }
                return false;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Saving Changes  Exception:{message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While Saving Changes  InnerException:{message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
