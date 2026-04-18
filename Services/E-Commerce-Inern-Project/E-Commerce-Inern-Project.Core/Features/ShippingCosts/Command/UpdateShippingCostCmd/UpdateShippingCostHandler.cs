using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd
{
    public class UpdateShippingCostHandler : IRequestHandler<UpdateShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public UpdateShippingCostHandler(IShippingCostsService shippingCostsService)
        {
            _shippingCostsService = shippingCostsService;
        }

        public async Task<Result<bool>> Handle(UpdateShippingCostRequest request, CancellationToken cancellationToken)
        {
            return await _shippingCostsService.UpdateShippingCost(request);
        }
    }
}
