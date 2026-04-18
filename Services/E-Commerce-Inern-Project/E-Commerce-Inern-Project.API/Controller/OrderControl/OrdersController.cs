using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllOrdersQ;
using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllUserOrdersQ;
using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetOrderByIDQ;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.OrderControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllOrders")]
        public async Task<Result<IEnumerable<OrderDetails>>> GetAllOrders()
        {
            return await _mediator.Send(new GetAllOrdersQuery());
        }
        [HttpGet("GetOrder/{OrderID}")]
        public async Task<Result<OrderDetails>> GetAllOrders(Guid OrderID)
        {
            return await _mediator.Send(new GetOrderByIDQuery(OrderID));
        }

        [HttpGet("GetAllUserOrders/{UserID}")]
        public async Task<Result<IEnumerable<OrderDetails>>> GetAllUserOrders(Guid UserID)
        {
            return await _mediator.Send(new GetAllUserOrdersQuery(UserID));
        }

        [HttpPost]
        public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpPut("UpdateOrderStatus")]
        public async Task<Result<bool>> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            return await _mediator.Send(request );
        }
        [HttpPut("UpdateOrder")]
        public async Task<Result<bool>> UpdateOrder(UpdateOrderRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
