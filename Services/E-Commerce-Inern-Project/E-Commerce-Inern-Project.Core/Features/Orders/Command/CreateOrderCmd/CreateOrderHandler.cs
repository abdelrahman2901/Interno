using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IOrderServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Result<OrderResponse>>
    {
        private readonly IOrderService _orderService;
        public CreateOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<Result<OrderResponse>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            return await _orderService.CreateOrder(request);
        }
    }
}
