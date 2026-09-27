using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.ICartItemRepo
{
    public interface ICartItemRepository
    {
        public Task<IEnumerable<CartItems>> GetCartItemsByCaerID(Guid CartID);
        public Task<CartItems?> GetCartItem(Guid CartItemID);
        public Task<bool> AddCartItemList(IEnumerable<CartItems> items);
        public Task<bool> SaveChanges();
    }
}
