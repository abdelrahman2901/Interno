using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ
{
    public class FilterProductsDetailsHandler : IRequestHandler<FilterProductsDetailsQuery, Result<IEnumerable<ProductDetails_vw>>>
    {
        private readonly IProductViewService _productService;
        public FilterProductsDetailsHandler(IProductViewService productService)
        {
            _productService = productService;
        }


        public async Task<Result<IEnumerable<ProductDetails_vw>>> Handle(FilterProductsDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.FilterProducts(request);
        }
    }
}
