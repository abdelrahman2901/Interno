using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Query.GetShippingCostDetailsQ
{
    public record GetShippingCostDetailsQuery(Guid ShippingID) : IRequest<Result<ShippingCostDetails>>;
}
