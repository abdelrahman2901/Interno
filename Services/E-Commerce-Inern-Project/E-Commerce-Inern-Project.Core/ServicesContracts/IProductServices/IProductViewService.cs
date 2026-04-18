using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices
{
    public interface IProductViewService
    {
        public Task<Result<ProductDetails_vw>> GetProductDetailsByID(Guid ProductID);
        public Task<Result<IEnumerable<ProductDetails_vw>>> GetAllProductsDetails();
        public Task<Result<IEnumerable<ProductDetails_vw>>> FilterProducts(FilterProductsDetailsQuery request);
    }
}
