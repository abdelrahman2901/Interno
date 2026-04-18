using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.OrderCouponDTO;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.DeleteCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponActivationCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetAllCouponsQ;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByCodeQ;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Query.GetCouponByIDQ;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.OrderCouponControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCouponController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderCouponController(IMediator mediator)
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
        [HttpPost]
        public async Task<Result<bool>> CreateCoupon(CreateCouponRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpPut("UpdateCoupon")]
        public async Task<Result<bool>> UpdateCoupon(UpdateCouponRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpPut("DeleteCoupon/{CouponID}")]
        public async Task<Result<bool>> DeleteCoupon(Guid CouponID)
        {
            return await _mediator.Send(new DeleteCouponRequest(CouponID));
        }
        [HttpPut("UpdateCouponActivation/{CouponID}")]
        public async Task<Result<bool>> UpdateCouponActivation(Guid CouponID)
        {
            return await _mediator.Send(new UpdateCouponActivationRequest(CouponID));
        }
    }
}
