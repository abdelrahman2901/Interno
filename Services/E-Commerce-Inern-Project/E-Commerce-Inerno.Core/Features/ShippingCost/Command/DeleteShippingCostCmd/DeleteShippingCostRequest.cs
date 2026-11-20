using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.DeleteShippingCostCmd
{
    public record DeleteShippingCostRequest(Guid ShippingCostID) : IRequest<Result<bool>>;
}
