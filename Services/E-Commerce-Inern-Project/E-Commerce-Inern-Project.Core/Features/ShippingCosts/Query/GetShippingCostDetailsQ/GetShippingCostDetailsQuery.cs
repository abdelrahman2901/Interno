using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetShippingCostDetailsQ
{
    public record GetShippingCostDetailsQuery(Guid ShippingID) : IRequest<Result<ShippingCostDetails>>;

}
