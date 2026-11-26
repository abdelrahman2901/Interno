using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Query.GetAllProductsDetailsQ
{
    public class GetAllProductDetailsHandler : IRequestHandler<GetAllProductDetailsQuery, Result<IEnumerable<ProductDetails_vw>>>
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IMapper _mapper;

        public GetAllProductDetailsHandler(IProductViewRepository productViewRepo, IMapper mapper)
        {
            _productViewRepo = productViewRepo;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDetails_vw>>> Handle(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productViewRepo.GetAllProductsDetails(cancellationToken);
            if (!products.Any())
            {
                return Result<IEnumerable<ProductDetails_vw>>.NotFound("No Products Was Found");
            }
            return Result<IEnumerable<ProductDetails_vw>>.Success(products.Select(p => _mapper.Map<ProductDetails_vw>(p)));
        }
    }
}
