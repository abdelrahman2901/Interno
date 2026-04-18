
using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo
{
    public interface IProductRepository
    {
         public   Task<IEnumerable<Product>> GetAllProducts();
         public   Task<Product?> GetProductByID_Tracking(Guid? ProductID);
         public   Task<Product?> GetProductByID_NoTracking(Guid? ProductID);
         public   Task<Product?> AddProduct(Product product);
         public   Task<bool> SaveChanges();

    }
}
