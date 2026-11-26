using E_Commerce_Interno.Domain.DTO.UserDTO;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        /// <summary>Projected query — pagination/filtering should happen on this IQueryable before materialization.</summary>
        IQueryable<ApplicationUser> GetAllUsersQuery();
        IQueryable<ApplicationUser> GetApplicationUserQueryByID(Guid? userid);
        Task<ApplicationUser?> GetApplicationUserByID(Guid? userid, bool AsTracking);
        Task<ApplicationUser?> GetApplicationUserByPhoneNumber(string Phone, bool AsTracking);
        Task<ApplicationUser?> GetApplicationUserByEmail(string email, bool AsTracking);
        Task<Guid> GetAdminID();
        Task<bool> IsEmailAlreadyRegister(string Email);
        Task<bool> SaveChanges();
    }
}
