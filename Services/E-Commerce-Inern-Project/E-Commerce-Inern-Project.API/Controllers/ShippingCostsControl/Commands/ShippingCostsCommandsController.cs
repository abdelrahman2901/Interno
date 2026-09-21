using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.DeleteShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.ShippingCostsControl.Commands
{
    [Route("api/ShippingCosts")]
    [ApiController]
    public class ShippingCostsCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShippingCostsCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Result<bool>> CreateShippingCost(CreateShippingCostRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("UpdateShippingCost")]
        public async Task<Result<bool>> UpdateShippingCost(UpdateShippingCostRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{ShippingCostID}")]
        public async Task<Result<bool>> DeleteShipping(Guid ShippingCostID)
        {
            return await _mediator.Send(new DeleteShippingCostRequest(ShippingCostID));
        }
    }
}
