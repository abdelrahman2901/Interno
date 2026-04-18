using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetShippingCostDetailsQ
{
    public class GetShippingCostDetailsHandler : IRequestHandler<GetShippingCostDetailsQuery, Result<ShippingCostDetails>>
    {
        private readonly IShippingCostsService _shippingCostsService;
        public GetShippingCostDetailsHandler(IShippingCostsService shippingCostsService)
        {
            _shippingCostsService = shippingCostsService;
        }

        public async Task<Result<ShippingCostDetails>> Handle(GetShippingCostDetailsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
