using E_Commerce_Inern_Project.Core.DTO.UserDTO;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.Identity;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ApplicationDbContext context , UserManager<ApplicationUser> usermanager , ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
            _UserManager = usermanager;
        }

        public async Task<IEnumerable<AuthUserDetailsDTO>> GetAllUsers()
        {
            try
            {

             return   await (from user in _context.Users
                       join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                       join role in _context.Roles on userRoles.RoleId equals role.Id
                       select new AuthUserDetailsDTO
                       {
                           Email = user.Email,
                           IsBlocked = user.IsBlocked,
                           PersonName = user.PersonName,
                           PhoneNumber = user.PhoneNumber,
                           Role = role.Name,
                           userID = user.Id
                       }).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving all users  Exception:{Message} ",ex.Message);
                if(ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ",ex.InnerException.Message);
                }
                return [];
            }
        }

        public async Task<string?> GetUserRole(ApplicationUser user)
        {
            try
            {
                return (await _UserManager.GetRolesAsync(user)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving user role  Exception:{Message} ", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception:{Message} ", ex.InnerException.Message);
                }
                return null;
            }
        }
     
        public async Task<ApplicationUser?> GetApplicationUserByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email &&!u.IsDeleted);

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

        public async Task<ApplicationUser?> GetApplicationUserByID(Guid? userID)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == userID && !u.IsDeleted);
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
                return await _context.Users.AnyAsync(u => u.Email == Email && !u.IsDeleted);
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

    }
}
