using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.AddressRepo
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<AddressRepository> _logger;
        public AddressRepository(ApplicationDBContext db, ILogger<AddressRepository> logger)
        {
            _context = db;
            _logger = logger;
        }
        public async Task<IEnumerable<Address>> GetAddressesByUserId(Guid userId)
        {
            try
            {
                return await _context.Address.AsNoTracking()
                    .Include(a => a.City)
                    .Include(a => a.Area)
                    .Where(a => a.UserID == userId && !a.IsDeleted)
                    .OrderByDescending(r => r.IsDefault)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Addresses  : {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while getting Addresses with ID: {Message}", ex.InnerException.Message);

                }
                return [];
            }
        }

        public async Task<bool> AddAddress(Address Address)
        {
            try
            {
                await _context.Address.AddAsync(Address);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Address with ID: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while getting Address with ID: {Message}", ex.InnerException.Message);

                }
                return false;
            }
        }

        public async Task<Address?> GetAddressByID(Guid AddressID, bool AsTracking)
        {
            try
            {
                IQueryable<Address> query = _context.Address;
                if (!AsTracking)
                {
                    query = query.AsNoTracking();
                }

                return await query.FirstOrDefaultAsync(a => a.AddressID == AddressID && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Address with ID: {massage}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while getting Address with ID: {massage}", ex.InnerException.Message);

                }
                return null;
            }
        }
       
        public async Task<Address?> GetDefaultAddress(bool AsTracking)
        {
            try
            {
                IQueryable<Address> query = _context.Address;
                if (!AsTracking)
                {
                    query = query.AsNoTracking();
                }
                return await query
                     .FirstOrDefaultAsync(a => a.IsDefault && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Default Address with ID: {massage}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while getting Default Address with ID: {massage}", ex.InnerException.Message);

                }
                return null;
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
                _logger.LogError(ex, "Error occurred while SaveChanges: {massage}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while SaveChanges: {massage}", ex.InnerException.Message);

                }
                return false;
            }
        }
    }
}
