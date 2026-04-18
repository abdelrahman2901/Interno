
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetAllShippingCostsQ
{
    public class GetAllShippingCostsHandler : IRequestHandler<GetAllShippingCostsRequest, Result<IEnumerable<ShippingCostDetails>>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public GetAllShippingCostsHandler(IShippingCostsService shippingCostsService)
        {
            _shippingCostsService = shippingCostsService;
        }

        public async Task<Result<IEnumerable<ShippingCostDetails>>> Handle(GetAllShippingCostsRequest request, CancellationToken cancellationToken)
        {
            return await _shippingCostsService.GetAllShippingCostDetails();
        }
    }
}
