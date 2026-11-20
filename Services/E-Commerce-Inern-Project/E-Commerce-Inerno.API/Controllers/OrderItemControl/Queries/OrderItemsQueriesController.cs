using E_Commerce_Inerno.Core.Features.OrderItem.Query.GetAllOrderItemsQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderItemsDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.OrderItemControl.Queries
{
    [Authorize(Roles = "Admin")]
    [Route("api/OrderItems")]
    [ApiController]
    public class OrderItemsQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemsQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllOrderItems")]
        public async Task<Result<PagedResult<OrderItemDetails>>> GetAllOrderItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            return await _mediator.Send(new GetAllOrderItemsQuery(pageNumber, pageSize));
        }
    }
}
