using E_Commerce_Inern_Project.Core.Features.ProductRate.Query.GetAllProductRatesForProductQ;
using E_Commerce_Inern_Project.Core.Features.ProductRate.Query.GetAllProductRatesQ;
using E_Commerce_Inern_Project.Core.Features.ProductRate.Query.GetUserRatingQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.ProductRateControl.Queries
{
    [Route("api/ProductRates")]
    [ApiController]
    public class ProductRatesQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductRatesQueriesController(IMediator mediator)
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
    }
}
