using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IPaymentRepo;
using E_Commerce_Inern_Project.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_Commerce_Inern_Project.Infrastructure.Repository.PaymentRepo
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(ApplicationDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Payments?> AddNewPayment(Payments payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding new payment : {message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception : {innerMessage}", ex.InnerException.Message);
                }
                return null;
            }
        } 
         
        

        public async Task<Payments?> GetLastPayment()
        {
            return await _context.Payments.OrderBy(i=>i.PaymentDate).LastOrDefaultAsync();
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
               return await _context.SaveChangesAsync()>0;
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
