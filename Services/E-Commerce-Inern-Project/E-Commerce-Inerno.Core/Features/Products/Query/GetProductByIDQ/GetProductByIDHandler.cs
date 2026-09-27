using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Products.Query.GetProductByIDQ
{
    public class GetProductByIDHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDetails_vw>>
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IMapper _mapper;

        public GetProductByIDHandler(IProductViewRepository productViewRepo, IMapper mapper)
        {
            _productViewRepo = productViewRepo;
            _mapper = mapper;
        }

        public async Task<Result<ProductDetails_vw>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productViewRepo.GetProductDetailsByID(request.ProductID);
            if (product == null)
            {
                return Result<ProductDetails_vw>.NotFound("Product Doesnt Exists");
            }

            return Result<ProductDetails_vw>.Success(_mapper.Map<ProductDetails_vw>(product));
        }
    }
}
