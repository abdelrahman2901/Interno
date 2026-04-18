using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace E_Commerce_Inern_Project.Infrastructure.Repository.ShippingCostRepo
{
    public class ShippingCostRepository : IShippingCostsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShippingCostRepository> _logger;
        public ShippingCostRepository(ApplicationDbContext context, ILogger<ShippingCostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ShippingCosts?> CreateNewShippingCosts(ShippingCosts NewShippingCosts)
        {
            try
            {
                await _context.ShippingCosts.AddAsync(NewShippingCosts);
                return NewShippingCosts;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While Craeting New Shipping Cost Exception: {Message}", ex.Message);
                if(ex.InnerException != null)
                {

                _logger.LogError("Error Occured While Craeting New Shipping Cost InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<IEnumerable<ShippingCosts>> GetAllShippingCosts()
        {
            try
            {
                return await _context.ShippingCosts.AsNoTracking().Where(r=>!r.IsDeleted).Include(r=>r.Area).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetAllShippingCosts Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetAllShippingCosts InnerException: {Message}", ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<ShippingCosts?> GetShippingCostDetailsByAreaID(Guid AreaID)
        {
            try 
            {
                return await _context.ShippingCosts.AsNoTracking().Include(r=>r.Area).FirstOrDefaultAsync(r => r.AraeID == AreaID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetShippingCostDetailsByAreaID Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetShippingCostDetailsByAreaID InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<ShippingCosts?> GetShippingCosts(Guid ShippingCostID)
        {
            try
            {
                return await _context.ShippingCosts.FirstOrDefaultAsync(r => r.ShippingCostID==ShippingCostID);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While GetShippingCosts Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While GetShippingCosts InnerException: {Message}", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync()>0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured While SaveChanges Exception: {Message}", ex.Message);
                if (ex.InnerException != null)
                {

                    _logger.LogError("Error Occured While SaveChanges InnerException: {Message}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
