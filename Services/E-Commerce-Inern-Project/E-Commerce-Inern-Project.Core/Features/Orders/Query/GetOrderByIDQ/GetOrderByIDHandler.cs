

using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Query.GetOrderByIDQ
{
    public class GetOrderByIDHandler : IRequestHandler<GetOrderByIDQuery, Result<OrderDetails>>
    {
        private readonly IOrderService _orderService;
        public GetOrderByIDHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result<OrderDetails>> Handle(GetOrderByIDQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrderByID(request.OrderID);
        }
    }
}
