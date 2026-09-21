using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrdersRepo
{
    public interface IOrdersRepository
    {
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task<IEnumerable<Order>> GetAllUserOrders(Guid CartID);
        public Task<Order> CreateOrder(Order order);
        public   Task<Order?> GetOrderByID_Tracking(Guid OrderID);
        public   Task<Order?> GetOrderByID_NoTracking(Guid OrderID);

        public Task<bool> SaveChanges();
    }
}