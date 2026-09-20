using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Enums;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRolesRepo
{
    public interface IUserRolesRepository
    {
        public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
        public Task<bool> RemoveUserFromRole(Guid UserID, RolesEnum NewRole);
        public Task<bool> AddUserToRole(Guid UserID, RolesEnum role);
    }
}
