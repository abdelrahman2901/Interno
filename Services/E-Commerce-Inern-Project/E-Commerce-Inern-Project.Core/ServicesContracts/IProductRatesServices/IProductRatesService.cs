using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices
{
    public interface IProductRatesService
    {
        public Task<Result<bool>> CreateProductRateList(IEnumerable<ProductRateRequest> request);
        public Task<Result<bool>> DeleteProductRate(Guid RateID);
        public Task<Result<IEnumerable<ProductRateResponse>>> GetProductRatesForProduct(Guid ProductID);
        public Task<Result<IEnumerable<ProductRateResponse>>> GetUserRating(Guid UserID);
        public Task<Result<IEnumerable<ProductRateResponse>>> GetAllProductRates();
        public Task<Result<ProductRateResponse>> GetProductRate(Guid RateID);
    }
}
