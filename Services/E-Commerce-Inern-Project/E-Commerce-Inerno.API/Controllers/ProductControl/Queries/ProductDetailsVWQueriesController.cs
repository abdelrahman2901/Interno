using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ;
using E_Commerce_Inerno.Core.Features.Products.Query.GetAllProductsDetailsQ;
using E_Commerce_Inerno.Core.Features.Products.Query.GetProductDetailsByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductsDTO;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ProductControl.Queries
{
    [AllowAnonymous]
    [Route("api/ProductDetailsVW")]
    [ApiController]
    public class ProductDetailsVWQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductDetailsVWQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<ProductDetails_vw>>> GetAllProductsDetails()
        {
            return await _mediator.Send(new GetAllProductDetailsQuery());
        }

        [HttpGet("{ProductID}")]
        public async Task<Result<ProductDetails_vw>> GetProductDetailsByID(Guid ProductID)
        {
            return await _mediator.Send(new GetProductDetailsByIDQuery(ProductID));
        }

        [HttpGet("FilterProducts")]
        public async Task<Result<GetProductsDto>> FilterProducts([FromQuery] FilterProductsDetailsQuery filters)
        {
            return await _mediator.Send(filters);
        }
    }
}
