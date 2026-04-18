using E_Commerce_Inern_Project.Core.Domain.Entity;


namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICartRepo
{
    public interface ICartRepository
    {
        public Task<Cart?> CreateCart(Cart Cart);
        public Task<Cart?> GetCartByUserID(Guid UserID);
        public Task<Cart> GetUserCarItemsQuery(Guid UserID);
        public Task<bool> IsUserHasCart(Guid UserID);
        public Task<bool> SaveChanges();
    }
}
