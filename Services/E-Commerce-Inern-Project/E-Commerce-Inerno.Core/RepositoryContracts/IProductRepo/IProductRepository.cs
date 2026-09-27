using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo
{
    public interface IProductRepository
    {
         public   Task<IEnumerable<Product>> GetAllProducts();
         public   Task<Product?> GetProductByID(Guid? ProductID, bool AsTracking);
         public   Task<bool> AddProduct(Product product);
         public   Task<bool> SaveChanges();

    }
}
