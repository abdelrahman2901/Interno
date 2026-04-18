using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.DeleteShippingCostCmd
{
    public class DeleteShippingCostHandler : IRequestHandler<DeleteShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public DeleteShippingCostHandler(IShippingCostsService shippingCostsService)
        {
            _shippingCostsService = shippingCostsService;
        }
        public async Task<Result<bool>> Handle(DeleteShippingCostRequest request, CancellationToken cancellationToken)
        {
            return await _shippingCostsService.DeleteShippingCost(request.ShippingCostID);
        }
    }
}
