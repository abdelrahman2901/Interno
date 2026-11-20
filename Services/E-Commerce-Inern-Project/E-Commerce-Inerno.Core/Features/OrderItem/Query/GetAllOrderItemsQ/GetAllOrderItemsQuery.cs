using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.OrderItem.Query.GetAllOrderItemsQ
{
    public record GetAllOrderItemsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<Result<PagedResult<OrderItemDetails>>>;
}
