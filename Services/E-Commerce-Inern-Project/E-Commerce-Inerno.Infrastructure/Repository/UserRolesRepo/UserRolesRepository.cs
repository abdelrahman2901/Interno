using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IUserRolesRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.UserRolesRepo
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly ApplicationDBContext _Context;
        private readonly ILogger<UserRolesRepository> _logger;
        public UserRolesRepository(ApplicationDBContext context, ILogger<UserRolesRepository> logger)
        {
            _Context = context;
            _logger = logger;
        }

        public async Task<bool> AddUserToRole(Guid UserID, RolesEnum role)
        {
            try
            {
                var Role = await _Context.ApplicationRole.FirstOrDefaultAsync(r => r.RoleEnumID == role);
                await _Context.ApplicationUserRoles.AddAsync(new ApplicationUserRoles { UserID = UserID, RoleID = Role.RoleID });
                return await _Context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Adding User to Role  Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Adding User to Role  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<bool> RemoveUserFromRole(Guid UserID, RolesEnum NewRole)
        {
            try
            {
                var Role = await _Context.ApplicationRole.FirstOrDefaultAsync(r => r.RoleEnumID == NewRole);
                var userRole = await _Context.ApplicationUserRoles.FirstOrDefaultAsync(ur => ur.UserID == UserID && ur.RoleID == Role.RoleID);
                if (userRole != null)
                {
                    userRole.IsDeleted = true;
                    return await _Context.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Removing User from Role  Exception : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Removing User from Role  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
            
        }

        public async Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _Context.ApplicationUserRoles.Where(r => r.UserID == user.UserID).Select(r => r.Role.RoleName).ToListAsync();
        }
    }
}