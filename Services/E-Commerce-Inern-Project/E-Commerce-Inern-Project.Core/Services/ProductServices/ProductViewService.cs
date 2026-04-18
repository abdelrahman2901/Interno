    using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IProductRepo;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;

namespace E_Commerce_Inern_Project.Core.Services.ProductServices
{
    public class ProductViewService : IProductViewService
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IMapper _mapper;
        public ProductViewService(IProductViewRepository productViewRepo, IMapper mapper)
        {
            _productViewRepo = productViewRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDetails_vw>>> FilterProducts(FilterProductsDetailsQuery request)
        {
            var products = await _productViewRepo.FilterProducts(request);
            if (!products.Any())
            {
                return Result<IEnumerable<ProductDetails_vw>>.NotFound("No Products Was Found");
            }
            return Result<IEnumerable<ProductDetails_vw>>.Success(products.Select(p => _mapper.Map<ProductDetails_vw>(p)));
        }

        public async Task<Result<IEnumerable<ProductDetails_vw>>> GetAllProductsDetails()
        {
            var products = await _productViewRepo.GetAllProductsDetails();
            if (!products.Any())
            {
                return Result<IEnumerable<ProductDetails_vw>>.NotFound("No Products Was Found");
            }
            return Result<IEnumerable<ProductDetails_vw>>.Success(products.Select(p=>_mapper.Map<ProductDetails_vw>(p)));
        }

        public async Task<Result<ProductDetails_vw>> GetProductDetailsByID(Guid ProductID)
        {
            var product = await _productViewRepo.GetProductDetailsByID(ProductID);
            if (product == null)
            {
                return Result<ProductDetails_vw>.NotFound("Product Doesnt Exists");
            }

            return Result<ProductDetails_vw>.Success(_mapper.Map<ProductDetails_vw>(product));
        }
    }
}
