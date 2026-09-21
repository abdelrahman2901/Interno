using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.CreateCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.DeleteCouponCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponActivationCmd;
using E_Commerce_Inern_Project.Core.Features.OrderCoupon.Command.UpdateCouponCmd;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.OrderCouponControl.Commands
{
    [Route("api/OrderCoupon")]
    [ApiController]
    public class OrderCouponCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderCouponCommandsController(IMediator mediator)
        {
            _mediator = mediator;
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
