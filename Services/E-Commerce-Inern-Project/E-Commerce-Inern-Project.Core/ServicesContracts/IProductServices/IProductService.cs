using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices
{
    public interface IProductService
    {
        public Task<Result<bool>> CreateProductAsync(ProductRequest request);
        public Task<Result<bool>> UpdateProductAsync(ProductUpdateRequest UpdateRequest);
        public Task<Result<bool>> DeleteProductAsync(Guid Productid);
        public Task<Result<bool>> UpdateProductRating(UpdateProductRatingRequest request);

    }
}
