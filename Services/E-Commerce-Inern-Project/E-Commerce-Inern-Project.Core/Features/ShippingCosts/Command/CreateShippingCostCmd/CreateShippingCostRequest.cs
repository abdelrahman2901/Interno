using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd
{
    public record CreateShippingCostRequest(decimal ShippingCost  ,Guid AraeID ) : IRequest<Result<bool>>;
}
