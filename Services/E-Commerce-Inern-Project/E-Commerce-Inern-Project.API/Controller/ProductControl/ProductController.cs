using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.DeleteProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductCommand;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.ProductControl
{
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

 
        [HttpPost]
        public async Task<Result<bool>> CreateNewProduct([FromForm] ProductRequest Request)
        {
            return await _mediator.Send(Request);  
        }
        [HttpPut]
        public async Task<Result<bool>> UpdateProduct(ProductUpdateRequest Request)
        {
            return await _mediator.Send(Request);
        }
        [HttpPut("UpdateProductRating")]
        public async Task<Result<bool>> UpdateProductRating(UpdateProductRatingRequest Request)
        {
            return await _mediator.Send(Request);
        }
        [HttpDelete("{ProductID}")]
        public async Task<Result<bool>> DeleteProduct(Guid ProductID)
        {
            return await _mediator.Send(new DeleteProductCmd(ProductID));
        }
    }
}
