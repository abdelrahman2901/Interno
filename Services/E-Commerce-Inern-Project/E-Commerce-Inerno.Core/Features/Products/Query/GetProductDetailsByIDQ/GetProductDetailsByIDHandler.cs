using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Query.GetProductDetailsByIDQ
{
    public class GetProductDetailsByIDHandler : IRequestHandler<GetProductDetailsByIDQuery, Result<ProductDetails_vw>>
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IMapper _mapper;

        public GetProductDetailsByIDHandler(IProductViewRepository productViewRepo, IMapper mapper)
        {
            _productViewRepo = productViewRepo;
            _mapper = mapper;
        }

        public async Task<Result<ProductDetails_vw>> Handle(GetProductDetailsByIDQuery request, CancellationToken cancellationToken)
        {
            var product = await _productViewRepo.GetProductDetailsByID(request.ProductID, cancellationToken);
            if (product == null)
            {
                return Result<ProductDetails_vw>.NotFound("Product Doesnt Exists");
            }

            return Result<ProductDetails_vw>.Success(_mapper.Map<ProductDetails_vw>(product));
        }
    }
}
