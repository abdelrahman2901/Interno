using E_Commerce_Inern_Project.Core.Common;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.DeleteShippingCostCmd
{
    public record DeleteShippingCostRequest(Guid ShippingCostID) : IRequest<Result<bool>>;
   
}
