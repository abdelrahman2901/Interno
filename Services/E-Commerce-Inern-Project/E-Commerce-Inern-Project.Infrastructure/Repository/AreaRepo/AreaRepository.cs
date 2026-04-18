using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAreaRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.AreaRepo
{
    public class AreaRepository : IAreaRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<AreaRepository> _logger;
        public AreaRepository(ApplicationDbContext context, ILogger<AreaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Area>> GetAllAreas()
        {
            try
            {

                return await _context.Area.AsNoTracking().Where(r => !r.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile InnerException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<Area?> GetArea_NoTracking(Guid id)
        {
            try
            {
                return await _context.Area.AsNoTracking().FirstOrDefaultAsync(a => a.AreaID == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile InnerException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }
        public async Task<Area?> GetArea_Tracking(Guid id)
        {
            try
            {
                return await _context.Area.FirstOrDefaultAsync(a => a.AreaID == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile GetArea_Tracking Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile GetArea_Tracking InnerException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        } 

        public async Task<bool> IsAreaExistsByName(string AreaName)
        {

            try
            {
                return await _context.Area.AnyAsync(c => c.AreaName == AreaName && !c.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile IsAreaExistsByName Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile IsAreaExistsByName InnerException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> AddArea(Area area)
        {
            try
            {
                await _context.Area.AddAsync(area);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured WHile AddArea Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile AddArea InnerException : {message}", ex.InnerException.Message);
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
                _logger.LogError("Error Occured WHile SaveChanges Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured WHile SaveChanges InnerException : {message}", ex.InnerException.Message);
                }

                return false;
            }
        }
    }
}
