using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ;
using E_Commerce_Inern_Project.Core.Features.Product.Query.GetAllProductsDetailsQ;
using E_Commerce_Inern_Project.Core.Features.Product.Query.GetProductDetailsByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E_Commerce_Inern_Project.API.Controller.ProductControl
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsVWController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductDetailsVWController(IMediator mediator)
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
        public async Task<Result<IEnumerable<ProductDetails_vw>>> FilterProducts([FromQuery] FilterProductsDetailsQuery filters)
        {
            return await _mediator.Send(filters);
        }
    }
}