using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllUserOrdersQ
{
    public class GetAllUserOrdersHandler : IRequestHandler<GetAllUserOrdersQuery, Result<IEnumerable<OrderDetails>>>
    {
        private readonly IOrderService _orderService;
        public GetAllUserOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result<IEnumerable<OrderDetails>>> Handle(GetAllUserOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetAllUserOrders(request.UserID);
        }
    }
}
