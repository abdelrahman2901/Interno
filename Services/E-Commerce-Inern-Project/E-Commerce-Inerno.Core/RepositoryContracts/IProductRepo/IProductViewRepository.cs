using E_Commerce_Inern_Project.Core.Features.Products.Query.FilterProductsDetailsQ;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo
{
    public interface IProductViewRepository
    {
        public Task<Product?> GetProductDetailsByID(Guid ProductID);
        public Task<IEnumerable<Product>> GetAllProductsDetails();
        public Task<IEnumerable<Product>> FilterProducts(FilterProductsDetailsQuery request);
    }
}
