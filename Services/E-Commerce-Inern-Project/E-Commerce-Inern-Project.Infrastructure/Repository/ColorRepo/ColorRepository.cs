using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.ColorsRepo
{
    public class ColorsRepository : IColorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ColorsRepository> _logger;
        public ColorsRepository(ApplicationDbContext context, ILogger<ColorsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Colors?> CreateColors(Colors color)
        {
            try
            {
                await _context.Color.AddAsync(color);
                return color;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a color. Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while creating a color. InnerExceptionException : {message}", ex.InnerException.Message);
                } 
                return null;
            }
        }

        public async Task<IEnumerable<Colors>> GetAllColors()
        {
            try
            {
                return await _context.Color.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Getting a colors. Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Getting a colors. InnerExceptionException : {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<Colors?> GetColor(Guid ColorsID)
        {
            try
            {
                return await _context.Color.FirstOrDefaultAsync(c => c.ColorID == ColorsID);

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Getting a color. Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Getting a color. InnerExceptionException : {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
            return    await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Saving Changes. Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Saving Changes. InnerExceptionException : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
