using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.GetProductByIDQ
{
    public class GetProductByIDHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDetails_vw>>
    {
        private readonly IProductViewService _ProductService;
        public GetProductByIDHandler(IProductViewService productService)
        {
            _ProductService = productService;
        }

        public async Task<Result<ProductDetails_vw>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _ProductService.GetProductDetailsByID(request.ProductID);
        }

    }
}
