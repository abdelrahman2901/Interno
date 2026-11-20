using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IProductRatesRepo;

public interface IProductRatesRepository
{
    public Task<IEnumerable<ProductRates>> GetAllProductRates();
    public Task<IEnumerable<ProductRates>> GetAllProductRatesForProduct(Guid ProductID);
    public Task<IEnumerable<ProductRates>> GetUserRating(Guid UserID);
    public Task<ProductRates?> GetProductRateByID(Guid id, bool AsTracking);
    public Task<bool> AddListAsync(IEnumerable<ProductRates> Rates);
    public Task<bool> isUserHasRatingForProduct(ProductRateRequest request);
    public Task<bool> SaveChanges();
}
