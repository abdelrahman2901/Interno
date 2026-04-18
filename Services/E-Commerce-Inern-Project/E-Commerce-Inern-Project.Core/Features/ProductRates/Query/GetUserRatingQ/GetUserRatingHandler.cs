using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetUserRatingQ
{
    partial class GetUserRatingHandler : IRequestHandler<GetUserRatingQuery, Result<IEnumerable<ProductRateResponse>>>
    {
        private readonly IProductRatesService _productRatesService;
        public GetUserRatingHandler(IProductRatesService productRatesService)
        {
            _productRatesService = productRatesService;
        }

        public async Task<Result<IEnumerable<ProductRateResponse>>> Handle(GetUserRatingQuery request, CancellationToken cancellationToken)
        {
            return await _productRatesService.GetUserRating(request.UserID);
        }
    }
}
