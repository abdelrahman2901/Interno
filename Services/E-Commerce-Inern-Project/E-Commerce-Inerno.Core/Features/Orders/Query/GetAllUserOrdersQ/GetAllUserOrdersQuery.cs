using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;


namespace E_Commerce_Inerno.Core.Features.Orders.Query.GetAllUserOrdersQ
{
    public record GetAllUserOrdersQuery(Guid UserID) : IRequest<Result<IEnumerable<OrderDetails>>>;
}
