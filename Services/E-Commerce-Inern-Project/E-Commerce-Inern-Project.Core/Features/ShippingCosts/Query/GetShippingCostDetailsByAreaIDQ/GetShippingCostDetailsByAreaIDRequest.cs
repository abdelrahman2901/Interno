using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetShippingCostDetailsByAreaIDQ
{
    public record GetShippingCostDetailsByAreaIDRequest(Guid AreaID) : IRequest<Result<ShippingCostDetails>>;
}
