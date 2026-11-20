using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.UpdateShippingCostCmd
{
    public record UpdateShippingCostRequest(Guid ShippingCostID, decimal ShippingCost, Guid AreaID, bool IsDeleted) : IRequest<Result<bool>>;
}
