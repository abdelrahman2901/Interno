using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ;
using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ.Models;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo
{
    public interface IProductViewRepository
    {
        /// <summary>Filtered/sorted projected query (avg/count from ProductRates) — apply Skip/Take before materialization, then CalculateRatingCls.</summary>
        Task<(IEnumerable<ProductRaw>, int total)>FilterProductsQuery(FilterProductsDetailsQuery request, int pageNumber, int pageSize,CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllProductsDetails(CancellationToken cancellationToken);
        Task<Product?> GetProductDetailsByID(Guid ProductID, CancellationToken cancellationToken);
    }
}
