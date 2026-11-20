using E_Commerce_Inerno.Core.RepositoryContracts.IOrderItemsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inerno.Core.Features.OrderItem.Query.GetAllOrderItemsQ
{
    public class GetAllOrderItemsHandler : IRequestHandler<GetAllOrderItemsQuery, Result<PagedResult<OrderItemDetails>>>
    {
        private readonly IOrderItemsRepoistory _orderItemsRepo;

        public GetAllOrderItemsHandler(IOrderItemsRepoistory orderItemsRepo)
        {
            _orderItemsRepo = orderItemsRepo;
        }

        public async Task<Result<PagedResult<OrderItemDetails>>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

            var query = _orderItemsRepo.GetAllOrderItemsQuery();
            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return Result<PagedResult<OrderItemDetails>>.NotFound("No order items found.");
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            foreach (var orderItem in items)
            {
                if (orderItem.Product == null) continue;
                var avg = orderItem.Product.Rating;
                var count = orderItem.Product.TotalRating;
                orderItem.Product.Rating = count == 0
                    ? 0
                    : CalculateRatingCls.CalcuateTheRating(count, avg);
            }

            return Result<PagedResult<OrderItemDetails>>.Success(
                PagedResult<OrderItemDetails>.Create(items, totalCount, pageNumber, pageSize));
        }
    }
}
