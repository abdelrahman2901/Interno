using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllUserOrdersQ
{
    public record GetAllUserOrdersQuery(Guid UserID) : IRequest<Result<IEnumerable<OrderDetails>>>;
}
