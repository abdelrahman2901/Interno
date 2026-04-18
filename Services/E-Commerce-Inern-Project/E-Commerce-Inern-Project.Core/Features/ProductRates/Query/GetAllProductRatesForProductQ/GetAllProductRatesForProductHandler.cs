using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesForProductQ
{
    public class GetAllProductRatesForProductHandler : IRequestHandler<GetAllProductRatesForProductQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesService _productRatesService;
        public GetAllProductRatesForProductHandler(IProductRatesService productRatesService)
        {
            _productRatesService = productRatesService;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetAllProductRatesForProductQuery request, CancellationToken cancellationToken)
        {
            return await _productRatesService.GetProductRatesForProduct(request.ProductID);
        }
    }
}
