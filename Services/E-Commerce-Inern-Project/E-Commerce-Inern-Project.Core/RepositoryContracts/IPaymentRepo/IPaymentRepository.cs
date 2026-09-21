using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IPaymentRepo
{
    public interface IPaymentRepository
    {
        public Task<Payments?> AddNewPayment(Payments payment);
        public Task<Payments?> GetLastPayment();
        public   Task<bool> SaveChanges();


    }
}
