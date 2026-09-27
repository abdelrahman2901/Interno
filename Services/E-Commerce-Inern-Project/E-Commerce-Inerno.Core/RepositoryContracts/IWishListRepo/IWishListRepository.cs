
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IWishListRepo
{
    public interface IWishListRepository
    {
        public Task<IEnumerable<WishList>> GetAllUserWishList(Guid UserID);
        public Task<WishList?> GetWishList(Guid WishListID);
        public Task<WishList?> GetWishListByProdID(Guid ProductID,Guid UserID);
        public Task<bool> CreateWishList(WishList WishList);
        public Task<bool> SaveChanges();
    }
}
