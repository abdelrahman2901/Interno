using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderDTO;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.CreateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderCmd;
using E_Commerce_Inern_Project.Core.Features.Orders.Command.UpdateOrderStatusCmd;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.OrderControl.Commands
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Result<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("UpdateOrderStatus")]
        public async Task<Result<bool>> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("UpdateOrder")]
        public async Task<Result<bool>> UpdateOrder(UpdateOrderRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
