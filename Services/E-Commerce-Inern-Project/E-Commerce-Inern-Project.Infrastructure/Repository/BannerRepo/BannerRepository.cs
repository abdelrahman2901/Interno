
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IBannerRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.BannerRepo
{
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BannerRepository> _logger;

        public BannerRepository(ApplicationDbContext context, ILogger<BannerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(BannerSlide slide)
        {
            try
            {
                await _context.BannerSlides.AddAsync(slide);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding BannerSlide Exception :{message}", ex.Message);
                if (ex.InnerException != null) 
                {
                    _logger.LogError("Error while adding BannerSlide InnerException: {message}", ex.InnerException.Message);
                }
                return false;
            }
        }

         

        public async Task<IEnumerable<BannerSlide>> GetAllBanners()
        {
            try
            {
                return await _context.BannerSlides.AsNoTracking().Include(r => r.BackgroundColor).Include(c => c.AccentColor).Where(x => !x.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding BannerSlide Exception :{message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error while adding BannerSlide InnerException: {message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<BannerSlide?> GetBannerByID_NoTracking(Guid id)
        {
            try
            {
                return await _context.BannerSlides.AsNoTracking().Include(r=>r.BackgroundColor).Include(c=>c.AccentColor).FirstOrDefaultAsync(x => x.BannerSlideID == id && !x.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while GetByID_NoTracking BannerSlide Exception :{message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error while GetByID_NoTracking BannerSlide InnerException: {message}", ex.InnerException.Message);
                }
                return null;
            }
        }
        public async Task<BannerSlide?> GetBannerByID_Traking(Guid id)
        {
            try
            {
                return await _context.BannerSlides.Include(r => r.BackgroundColor).Include(c => c.AccentColor).FirstOrDefaultAsync(x => x.BannerSlideID == id && !x.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding BannerSlide Exception:{message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error while adding BannerSlide InnerException: {message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
               return  await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while SaveChanges BannerSlide Exception :{message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error while SaveChanges BannerSlide InnerException: {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
