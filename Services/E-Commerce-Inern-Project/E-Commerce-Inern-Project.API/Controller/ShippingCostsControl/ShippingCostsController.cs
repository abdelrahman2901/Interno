using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.DeleteShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetAllShippingCostsQ;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Query.GetShippingCostDetailsByAreaIDQ;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.ShippingCostsControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingCostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ShippingCostsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<Result<IEnumerable<ShippingCostDetails>>> GetShippingCostsDetails()
        {
            return await _mediator.Send(new GetAllShippingCostsRequest());
        }
       
        [HttpGet("GetShippingCostDetailsByAreaID/{AreaID}")]
        public async Task<Result<ShippingCostDetails>> GetShippingCostDetailsByAreaID(Guid AreaID)
        {
            return await _mediator.Send(new GetShippingCostDetailsByAreaIDRequest(AreaID));
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
