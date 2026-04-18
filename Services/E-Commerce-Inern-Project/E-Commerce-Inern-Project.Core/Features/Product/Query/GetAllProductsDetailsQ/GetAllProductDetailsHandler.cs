using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.GetAllProductsDetailsQ
{
    public class GetAllProductDetailsHandler : IRequestHandler<GetAllProductDetailsQuery, Result<IEnumerable<ProductDetails_vw>>>
    {
        private readonly IProductViewService _productService;
        public GetAllProductDetailsHandler(IProductViewService productService)
        {
            _productService = productService;
        }

        public Task<Result<IEnumerable<ProductDetails_vw>>> Handle(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
        {
            return _productService.GetAllProductsDetails();
        }
    }
}
