using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.DTO.UserDTO;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ApplicationDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            try
            {
                return await _context.ApplicationUser.Include(r => r.Roles).ThenInclude(r => r.Role).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving all ApplicationUser  Exception:{Message} ", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ", ex.InnerException.Message);
                }
                return [];
            }
        }

        public IQueryable<AuthUserDetailsDTO> GetAllUsersQuery()
        {
            return _context.ApplicationUser
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.PersonName)
                .Select(u => new AuthUserDetailsDTO
                {
                    userID = u.UserID,
                    Email = u.Email,
                    PersonName = u.PersonName,
                    PhoneNumber = u.PhoneNumber,
                    IsBlocked = u.IsBlocked,
                    Roles = u.Roles.Select(r => r.Role.RoleEnumID).ToList()
                });
        }

        public async Task<ApplicationUser?> GetApplicationUserByEmail(string email, bool AsTracking)
        {
            try
            {
                IQueryable<ApplicationUser> query = AsTracking ? _context.ApplicationUser : _context.ApplicationUser.AsNoTracking();
                return await query.Include(r => r.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving  user  Exception:{Message} ", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<ApplicationUser?> GetApplicationUserByPhoneNumber(string Phone, bool AsTracking)
        {
            try
            {
                IQueryable<ApplicationUser> query = AsTracking ? _context.ApplicationUser : _context.ApplicationUser.AsNoTracking();

                return await query.Include(r => r.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.PhoneNumber == Phone && !u.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving  user  Exception:{Message} ", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<ApplicationUser?> GetApplicationUserByID(Guid? userID, bool AsTracking)
        {
            try
            {
                IQueryable<ApplicationUser> query = AsTracking ? _context.ApplicationUser : _context.ApplicationUser.AsNoTracking();

                return await query.Include(r => r.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u => u.UserID == userID && !u.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving  user  Exception:{Message} ", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ", ex.InnerException.Message);
                }
                return null;
            }
        }

        public async Task<bool> IsEmailAlreadyRegister(string Email)
        {
            try
            {
                return await _context.ApplicationUser.AnyAsync(u => u.Email == Email && !u.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Checking if the Email is Regitsered Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Checking if the Email is Regitsered Exception : {message}", ex.InnerException.Message);
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
                _logger.LogError("An error occurred while Saving Changes  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Saving Changes  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<Guid> GetAdminID()
        {
            return await _context.ApplicationUser.AsNoTracking().Where(r => r.Roles.Any(r => r.Role.RoleEnumID == RolesEnum.Admin)).Select(r => r.UserID).FirstOrDefaultAsync();
        }
    }
}
