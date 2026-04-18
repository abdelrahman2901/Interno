using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllOrdersQ
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, Result<IEnumerable<OrderDetails>>>
    {
        private readonly IOrderService _orderService;
        public GetAllOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result<IEnumerable<OrderDetails>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetAllOrders();
        }
    }
}
