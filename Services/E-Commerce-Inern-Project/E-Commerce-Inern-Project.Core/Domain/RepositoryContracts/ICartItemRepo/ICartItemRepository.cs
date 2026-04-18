using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartItemRepo
{
    public interface ICartItemRepository
    {
        public Task<CartItems?> GetCartItem(Guid CartItemID);
        public Task<CartItems?> CreateCartItem(CartItems item);
        public   Task<IEnumerable<CartItems>> GetCartItemsByCaerID(Guid CartID);
        public Task<bool> AddCartItemList(IEnumerable<CartItems >items);
        public Task<bool> SaveChanges();


    }
}
