using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.CreateShippingCostCmd
{
    public record CreateShippingCostRequest(decimal ShippingCost, Guid AraeID) : IRequest<Result<bool>>;
}
