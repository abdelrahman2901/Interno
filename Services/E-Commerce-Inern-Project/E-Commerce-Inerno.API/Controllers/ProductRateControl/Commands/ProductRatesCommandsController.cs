using E_Commerce_Inerno.Core.Features.ProductRate.Command.CreateProductRateCmd;
using E_Commerce_Inerno.Core.Features.ProductRate.Command.DeleteProductRateCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ProductRateControl.Commands
{
    [Route("api/ProductRates")]
    [ApiController]
    public class ProductRatesCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductRatesCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Result<bool>> CreateProductRateList(IEnumerable<ProductRateRequest> request)
        {
            return await _mediator.Send(new CreateProductRateRequest(request));
        }

        [HttpPut("{RateID}")]
        public async Task<Result<bool>> DeleteProductRate(Guid RateID)
        {
            return await _mediator.Send(new DeleteProductRateRequest(RateID));
        }
    }
}
