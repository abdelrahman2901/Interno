using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Command.CreateProductRateCmd;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Command.DeleteProductRateCmd;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.ProductRateControl.Commands
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
