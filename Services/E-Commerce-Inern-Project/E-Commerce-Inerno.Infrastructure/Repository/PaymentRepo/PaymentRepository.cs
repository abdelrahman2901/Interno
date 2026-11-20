using E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inerno.Infrastructure.ApplicationDbContext;
using E_Commerce_Interno.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(ApplicationDBContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddNewPayment(Payments payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding new payment : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception : {innerMessage}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<Payments?> GetLastPayment()
        {
            return await _context.Payments.OrderBy(i => i.PaymentDate).LastOrDefaultAsync();
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding new payment : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception : {innerMessage}", ex.InnerException.Message);
                }
                return false;
            }
        }
    }
}
