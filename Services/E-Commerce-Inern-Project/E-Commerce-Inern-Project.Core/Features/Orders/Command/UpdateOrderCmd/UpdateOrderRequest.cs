using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Enums;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd
{
    public record UpdateOrderRequest(Guid OrderID , Guid AddressID,Guid ShippingCostID, OrderStatus OrderStatus) : IRequest<Result<bool>>;
}
