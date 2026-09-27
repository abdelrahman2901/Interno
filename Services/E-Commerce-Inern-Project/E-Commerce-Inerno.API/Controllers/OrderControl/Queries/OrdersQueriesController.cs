using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllOrdersQ;
using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetAllUserOrdersQ;
using E_Commerce_Inern_Project.Core.Features.Orders.Query.GetOrderByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.OrderControl.Queries
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersQueriesController(IMediator mediator)
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
    }
}
