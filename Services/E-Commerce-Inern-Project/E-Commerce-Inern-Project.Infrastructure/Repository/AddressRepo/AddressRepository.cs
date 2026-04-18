using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAddressRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.AddressRepo
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressRepository> _logger;
        public AddressRepository(ApplicationDbContext db, ILogger<AddressRepository> logger)
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
                    .OrderByDescending(r=>r.IsDefault)
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

        public async Task<Address?> AddAddress(Address Address)
        {
            try
            {
                await _context.Address.AddAsync(Address);
                return Address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting Address with ID: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex, "Error occurred while getting Address with ID: {Message}", ex.InnerException.Message);

                }
                return null;
            }
        }
   
    

        public async Task<Address?> GetAddressByID_NoTracking(Guid AddressID)
        {
            try
            {
                return await _context.Address.AsNoTracking()
                     .FirstOrDefaultAsync(a => a.AddressID == AddressID && !a.IsDeleted);
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
        public async Task<Address?> GetAddressByID_Tracking(Guid AddressID)
        {
            try
            {
                return await _context.Address
                     .FirstOrDefaultAsync(a => a.AddressID == AddressID && !a.IsDeleted);
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

        public async Task<Address?> GetDefaultAddress_Tracking()
        {
            try
            {
                return await _context.Address
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
