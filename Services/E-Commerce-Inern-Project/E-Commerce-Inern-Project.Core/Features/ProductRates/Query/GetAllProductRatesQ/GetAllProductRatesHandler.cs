using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesQ
{
    public class GetAllProductRatesHandler : IRequestHandler<GetAllProductRatesQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesService _productRatesService;
        public GetAllProductRatesHandler(IProductRatesService productRatesService)
        {
            _productRatesService = productRatesService;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetAllProductRatesQuery request, CancellationToken cancellationToken)
        {
            return await _productRatesService.GetAllProductRates();
        }
    }
}
