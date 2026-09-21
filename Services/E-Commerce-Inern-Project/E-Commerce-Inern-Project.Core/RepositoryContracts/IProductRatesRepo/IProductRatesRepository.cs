using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRatesRepo
{
    public interface IProductRatesRepository
    {
     public   Task<IEnumerable<ProductRates>> GetAllProductRates();
     public   Task<IEnumerable<ProductRates>> GetAllProductRatesForProduct(Guid ProductID);
     public   Task<IEnumerable<ProductRates>> GetUserRating(Guid UserID);
     public   Task<ProductRates?> GetProductRateByID_NoTracking(Guid id);
     public   Task<ProductRates?> GetProductRateByID_Traking(Guid id);
     public   Task<bool> AddListAsync(IEnumerable<ProductRates> Rates);
     public   Task<bool> isUserHasRatingForProduct(ProductRateRequest request);
     public   Task<bool> SaveChanges();
    }
}
