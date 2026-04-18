using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.TransectionRepo
{
    public class TransectionRepository : ITransectionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransectionRepository> _logger;
        public TransectionRepository(ApplicationDbContext context, ILogger<TransectionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            try
            {
                return await _context.Database.BeginTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occure While Begginning Transection Exception: {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Error Occure While Begginning Transection InnerException: {message}", ex.InnerException.Message);
                }
                return null;
            }
        }
    }
}
