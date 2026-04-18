using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICityRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.CityRepo
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CityRepository> _logger;
        public CityRepository(ApplicationDbContext context, ILogger<CityRepository> logger)
        {
            _context = context;
            _logger = logger; 
        } 

        public async Task<City?> AddCity(City City)
        {
            try
            {
                await _context.City.AddAsync(City);
                return City;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While AddCity Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While AddCity InnerException: {message}", ex.InnerException.Message);

                }
                return null;
            }
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            try
            {

               return await _context.City.AsNoTracking().Where(r=>!r.IsDeleted).ToListAsync();
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

        public async Task<City?> GetCity_NoTracking(Guid id)
        {
            try
            {
            return await _context.City.AsNoTracking().FirstOrDefaultAsync(c => c.CityID == id && !c.IsDeleted);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCity_NoTracking Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetCity_NoTracking InnerException: {message}", ex.InnerException.Message);

                }
                return null;
            }
        }
        public async Task<City?> GetCity_Tracking(Guid id)
        {
            try
            {
            return await _context.City.FirstOrDefaultAsync(c => c.CityID == id && !c.IsDeleted);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetCity_Tracking Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occured While GetCity_Tracking InnerException: {message}", ex.InnerException.Message);

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
            }catch(Exception ex)
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
