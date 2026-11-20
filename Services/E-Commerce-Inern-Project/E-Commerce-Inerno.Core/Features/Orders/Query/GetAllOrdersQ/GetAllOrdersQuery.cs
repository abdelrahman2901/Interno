using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Orders.Query.GetAllOrdersQ
{
    public record GetAllOrdersQuery(int PageNumber = 1, int PageSize = 20) : IRequest<Result<PagedResult<OrderDetails>>>;
}
