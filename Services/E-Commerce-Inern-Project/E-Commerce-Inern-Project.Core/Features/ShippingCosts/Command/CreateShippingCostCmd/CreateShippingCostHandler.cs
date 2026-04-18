using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd
{
    public class CreateShippingCostHandler : IRequestHandler<CreateShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public CreateShippingCostHandler(IShippingCostsService shippingCostsService)
        {
                _shippingCostsService = shippingCostsService;
        }
        public async Task<Result<bool>> Handle(CreateShippingCostRequest request, CancellationToken cancellationToken)
        {
            return await _shippingCostsService.CreateNewShippingCost(request);
        }
    }
}
