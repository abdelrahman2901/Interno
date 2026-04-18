using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetShippingCostDetailsByAreaIDQ
{
    public class GetShippingCostDetailsByAreaIDHandler : IRequestHandler<GetShippingCostDetailsByAreaIDRequest, Result<ShippingCostDetails>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public GetShippingCostDetailsByAreaIDHandler(IShippingCostsService shippingCostsService)
        {
            _shippingCostsService = shippingCostsService;
        }
        public async Task<Result<ShippingCostDetails>> Handle(GetShippingCostDetailsByAreaIDRequest request, CancellationToken cancellationToken)
        {
            return await _shippingCostsService.GetShippingCostDetailsByAreaID(request.AreaID);
        }
    }
}
