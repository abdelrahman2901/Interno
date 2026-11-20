using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Query.GetAllShippingCostsQ
{
    public record GetAllShippingCostsRequest() : IRequest<Result<IEnumerable<ShippingCostDetails>>>;
}
