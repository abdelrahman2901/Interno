using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;


namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllOrdersQ
{
    public record GetAllOrdersQuery() : IRequest<Result<IEnumerable<OrderDetails>>>;

}