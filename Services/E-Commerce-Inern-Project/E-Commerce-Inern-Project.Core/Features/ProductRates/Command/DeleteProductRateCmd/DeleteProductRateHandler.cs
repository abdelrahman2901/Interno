using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Command.DeleteProductRateCmd
{
    public class DeleteProductRateHandler : IRequestHandler<DeleteProductRateRequest, Result<bool>>
    {
        private readonly IProductRatesService _productRatesService;
        public DeleteProductRateHandler(IProductRatesService productRatesService)
        {
            _productRatesService = productRatesService;
        }
        public async Task<Result<bool>> Handle(DeleteProductRateRequest request, CancellationToken cancellationToken)
        {
            return await _productRatesService.DeleteProductRate(request.RateID);
        }
    }
}
