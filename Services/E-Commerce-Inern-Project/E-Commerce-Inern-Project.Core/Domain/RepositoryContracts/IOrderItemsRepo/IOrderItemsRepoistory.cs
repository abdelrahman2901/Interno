using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IOrderItemsRepo
{
    public interface IOrderItemsRepoistory
    {
        public Task<bool> AddOrderItem(OrderItems item);
        public   Task<bool> AddOrderItemList(IEnumerable<OrderItems> items);
        public Task<OrderItems?> GetOrderItem(Guid OrderItemID);
        public Task<bool> SaveChange();
    }
}
