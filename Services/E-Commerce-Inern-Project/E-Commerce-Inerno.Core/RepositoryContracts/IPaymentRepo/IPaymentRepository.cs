using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IPaymentRepo
{
    public interface IPaymentRepository
    {
        public Task<bool> AddNewPayment(Payments payment);
        public Task<Payments?> GetLastPayment();
        public Task<bool> SaveChanges();
    }
}
