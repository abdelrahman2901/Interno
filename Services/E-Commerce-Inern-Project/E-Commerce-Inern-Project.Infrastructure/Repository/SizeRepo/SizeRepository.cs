using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ISizeRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.SizeRepo
{
    public class SizeRepository : ISizeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SizeRepository> _logger;

        public SizeRepository(ApplicationDbContext context, ILogger<SizeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Size?> CreateSize(Size size)
        {
            try
            {
                await _context.Size.AddAsync(size);
                return size;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a size Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while creating a size Exception :{Message}", ex.InnerException.Message);
                } 
                return null;
            }
            }

      
        public async Task<IEnumerable<Size>> GetAllSizes()
        {
            try
            {
                return await _context.Size.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Getting a sizes Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while  Getting a sizes Exception :{Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<Size?> GetSize(Guid SizeID)
        {
            try
            {
                return await _context.Size.FirstOrDefaultAsync(s=>s.SizeID== SizeID);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Getting a size Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while  Getting a size Exception :{Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
             return   await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while SavingChanges  Exception :{Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while  SavingChanges Exception :{Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
