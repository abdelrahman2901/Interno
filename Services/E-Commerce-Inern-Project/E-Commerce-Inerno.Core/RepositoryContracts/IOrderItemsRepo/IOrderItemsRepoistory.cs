using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IOrderItemsRepo
{
    public interface IOrderItemsRepoistory
    {
        Task<bool> AddOrderItem(OrderItems item);
        Task<bool> AddOrderItemList(IEnumerable<OrderItems> items);
        Task<OrderItems?> GetOrderItem(Guid OrderItemID);
        /// <summary>Projected query — pagination/filtering should happen on this IQueryable before materialization.</summary>
        IQueryable<OrderItemDetails> GetAllOrderItemsQuery();
        Task<bool> SaveChange();
    }
}
