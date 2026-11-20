using E_Commerce_Inerno.Core.Domain.RepositoryContracts.IOrdersRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inerno.Core.Features.Orders.Query.GetAllOrdersQ
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, Result<PagedResult<OrderDetails>>>
    {
        private readonly IOrdersRepository _ordersRepository;

        public GetAllOrdersHandler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Result<PagedResult<OrderDetails>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

            var query = _ordersRepository.GetAllOrdersQuery();
            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return Result<PagedResult<OrderDetails>>.NotFound("Orders Wasnt Found.");
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            foreach (var order in items)
            {
                if (order.OrderItems == null) continue;
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.Product == null) continue;
                    var avg = orderItem.Product.Rating;
                    var count = orderItem.Product.TotalRating;
                    orderItem.Product.Rating = count == 0
                        ? 0
                        : CalculateRatingCls.CalcuateTheRating(count, avg);
                }
            }

            return Result<PagedResult<OrderDetails>>.Success(
                PagedResult<OrderDetails>.Create(items, totalCount, pageNumber, pageSize));
        }
    }
}
