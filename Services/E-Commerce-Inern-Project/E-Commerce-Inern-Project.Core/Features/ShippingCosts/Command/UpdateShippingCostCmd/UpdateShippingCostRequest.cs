using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd
{
    public record UpdateShippingCostRequest(Guid ShippingCostID, decimal ShippingCost, Guid AreaID,bool IsDeleted) : IRequest<Result<bool>>;
}
