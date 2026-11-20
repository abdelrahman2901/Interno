using E_Commerce_Inerno.Core.RepositoryContracts.IAuthRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace E_Commerce_Inerno.Infrastructure.Repository.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _Context;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(ApplicationDBContext Context, ILogger<AuthRepository> logger)
        {
            _Context = Context;
            _logger = logger;
        }

        public async Task<bool> IsPasswordCorrect(Guid UserID, string Password)
        {
            try
            {
                return await _Context.ApplicationUser.AnyAsync(r => r.UserID == UserID && r.Password == Password && !r.IsDeleted && !r.IsBlocked);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while  Checking if the password is correct Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Checking if the password is correct  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
       
        public async Task<bool> RegisterUser(ApplicationUser NewUser)
        {
            try
            {
                var result = await _Context.AddAsync(NewUser);

                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while Registering New User  Exception : {message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("An error occurred while Registering New User  Exception : {message}", ex.InnerException.Message);
                }
                return false;
            }
        }
        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _Context.SaveChangesAsync() > 0;
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
   
    }
}
