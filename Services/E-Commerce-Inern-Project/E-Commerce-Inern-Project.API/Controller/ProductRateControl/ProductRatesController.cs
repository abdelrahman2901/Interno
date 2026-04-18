using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Command.CreateProductRateCmd;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Command.DeleteProductRateCmd;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesForProductQ;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesQ;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetUserRatingQ;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.ProductRateControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{ProductID}")]
        public async Task<Result<IEnumerable<ProductRateResponse>>> GetProductRatesForProduct(Guid ProductID)
        {
            return await _mediator.Send(new GetAllProductRatesForProductQuery(ProductID));
        }

        [HttpGet]
        public async Task<Result<IEnumerable<ProductRateResponse>>> GetAllProductRates()
        {
            return await _mediator.Send(new GetAllProductRatesQuery());
        }
        [HttpGet("GetUserRating/{UserID}")]
        public async Task<Result<IEnumerable<ProductRateResponse>>> GetUserRating(Guid UserID)
        {
            return await _mediator.Send(new GetUserRatingQuery(UserID));
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
