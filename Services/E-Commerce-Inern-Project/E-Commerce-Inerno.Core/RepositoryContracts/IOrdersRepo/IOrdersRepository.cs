using E_Commerce_Interno.Domain.DTO.OrderDTO;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IOrdersRepo
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        /// <summary>Projected query — pagination/filtering should happen on this IQueryable before materialization.</summary>
        IQueryable<OrderDetails> GetAllOrdersQuery();
        Task<IEnumerable<Order>> GetAllUserOrders(Guid UserID);
        Task<Order?> GetOrderByID(Guid OrderID, bool AsTracking);
        Task<bool> CreateOrder(Order order);
        Task<bool> SaveChanges();
    }
}
