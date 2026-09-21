using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesForProductQ;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesQ;
using E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetUserRatingQ;
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
