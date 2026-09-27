using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo
{
    public interface IOrdersRepository
    {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<IEnumerable<Order>> GetAllUserOrders(Guid CartID);
        public Task<Order?> GetOrderByID(Guid OrderID,bool AsTracking);
        public Task<bool> CreateOrder(Order order);
        public Task<bool> SaveChanges();
    }
}