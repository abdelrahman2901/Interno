using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Products.Query.FilterProductsDetailsQ
{
    public class FilterProductsDetailsHandler : IRequestHandler<FilterProductsDetailsQuery, Result<IEnumerable<ProductDetails_vw>>>
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IMapper _mapper;

        public FilterProductsDetailsHandler(IProductViewRepository productViewRepo, IMapper mapper)
        {
            _productViewRepo = productViewRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDetails_vw>>> Handle(FilterProductsDetailsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productViewRepo.FilterProducts(request);
            if (!products.Any())
            {
                return Result<IEnumerable<ProductDetails_vw>>.NotFound("No Products Was Found");
            }
            return Result<IEnumerable<ProductDetails_vw>>.Success(products.Select(p => _mapper.Map<ProductDetails_vw>(p)));
        }
    }
}
