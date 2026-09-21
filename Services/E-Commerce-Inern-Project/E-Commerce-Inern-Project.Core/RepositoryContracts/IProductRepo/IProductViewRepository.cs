using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo
{
    public interface IProductViewRepository
    {
        public Task<Product?> GetProductDetailsByID(Guid ProductID);
        public Task<IEnumerable<Product>> GetAllProductsDetails();
        public Task<IEnumerable<Product>> FilterProducts(FilterProductsDetailsQuery request);
    }
}
