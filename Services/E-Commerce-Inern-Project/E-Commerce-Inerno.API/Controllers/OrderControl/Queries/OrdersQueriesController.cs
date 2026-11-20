using E_Commerce_Inerno.Core.Features.Orders.Query.GetAllOrdersQ;
using E_Commerce_Inerno.Core.Features.Orders.Query.GetAllUserOrdersQ;
using E_Commerce_Inerno.Core.Features.Orders.Query.GetOrderByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.OrderControl.Queries
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

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<Result<PagedResult<OrderDetails>>> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return await _mediator.Send(new GetAllOrdersQuery(pageNumber, pageSize));
        }

        [HttpGet("GetOrder/{OrderID}")]
        public async Task<Result<OrderDetails>> GetOrderById(Guid OrderID)
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
