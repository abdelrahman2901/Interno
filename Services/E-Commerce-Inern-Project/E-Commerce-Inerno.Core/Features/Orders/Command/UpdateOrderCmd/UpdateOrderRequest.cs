using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Orders.Command.UpdateOrderCmd
{
    public record UpdateOrderRequest(Guid OrderID , Guid AddressID,Guid ShippingCostID, OrderStatus OrderStatus) : IRequest<Result<bool>>;
}
