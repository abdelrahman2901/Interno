using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Enums;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd
{
    public record UpdateOrderStatusRequest(Guid OrderID, OrderStatus NewStatus) : IRequest<Result<bool>>;
}
