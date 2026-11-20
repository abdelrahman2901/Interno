using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ;
using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ.Models;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo
{
    public interface IProductViewRepository
    {
        /// <summary>Filtered/sorted projected query (avg/count from ProductRates) — apply Skip/Take before materialization, then CalculateRatingCls.</summary>
        IQueryable<ProductRaw> FilterProductsQuery(FilterProductsDetailsQuery request);
        Task<IEnumerable<Product>> GetAllProductsDetails();
        Task<Product?> GetProductDetailsByID(Guid ProductID);
    }
}
