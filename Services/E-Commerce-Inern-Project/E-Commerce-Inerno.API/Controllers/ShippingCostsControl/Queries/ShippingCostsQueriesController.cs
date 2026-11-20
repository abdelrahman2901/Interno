using E_Commerce_Inerno.Core.Features.ShippingCost.Query.GetAllShippingCostsQ;
using E_Commerce_Inerno.Core.Features.ShippingCost.Query.GetShippingCostDetailsByAreaIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ShippingCostsControl.Queries
{
    [Route("api/ShippingCosts")]
    [ApiController]
    public class ShippingCostsQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShippingCostsQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<ShippingCostDetails>>> GetShippingCostsDetails()
        {
            return await _mediator.Send(new GetAllShippingCostsRequest());
        }

        [HttpGet("[action]/{AreaID}")]
        public async Task<Result<ShippingCostDetails>> GetShippingCostDetailsByAreaID(Guid AreaID)
        {
            return await _mediator.Send(new GetShippingCostDetailsByAreaIDRequest(AreaID));
        }
    }
}
