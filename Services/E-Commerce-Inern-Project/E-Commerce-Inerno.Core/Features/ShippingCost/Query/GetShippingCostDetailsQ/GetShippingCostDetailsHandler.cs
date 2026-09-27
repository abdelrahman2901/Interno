using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCost.Query.GetShippingCostDetailsQ
{
    public class GetShippingCostDetailsHandler : IRequestHandler<GetShippingCostDetailsQuery, Result<ShippingCostDetails>>
    {
        public Task<Result<ShippingCostDetails>> Handle(GetShippingCostDetailsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
