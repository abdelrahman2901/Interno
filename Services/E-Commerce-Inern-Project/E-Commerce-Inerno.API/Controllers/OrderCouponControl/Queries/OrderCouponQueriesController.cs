using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetAllCouponsQ;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByCodeQ;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.OrderCouponDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.OrderCouponControl.Queries
{
    [Route("api/OrderCoupon")]
    [ApiController]
    public class OrderCouponQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderCouponQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<OrderCouponResponse>>> GetCoupons()
        {
            return await _mediator.Send(new GetAllCouponsQuery());
        }

        [HttpGet("{CouponID}")]
        public async Task<Result<OrderCouponResponse>> GetCoupon(Guid CouponID)
        {
            return await _mediator.Send(new GetCouponByIDQuery(CouponID));
        }

        [HttpGet("GetCouponByCode")]
        public async Task<Result<OrderCouponResponse>> GetCouponByCode(string CouponCode)
        {
            return await _mediator.Send(new GetCouponByCodeQuery(CouponCode));
        }
    }
}
