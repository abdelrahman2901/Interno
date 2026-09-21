using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IWishListRepo
{
    public interface IWishListRepository
    {
        public Task<IEnumerable<WishList>> GetAllUserWishList(Guid UserID);
        public Task<WishList?> GetWishList(Guid WishListID);
        public Task<WishList?> GetWishListByProdID(Guid ProductID,Guid UserID);
        public Task<WishList?> CreateWishList(WishList WishList);
        public Task<bool> SaveChanges();
    }
}
