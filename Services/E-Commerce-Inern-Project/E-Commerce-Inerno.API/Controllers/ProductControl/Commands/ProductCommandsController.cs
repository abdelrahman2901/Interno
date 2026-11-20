using E_Commerce_Inerno.Core.Features.Products.Commands.CreateProductCommand;
using E_Commerce_Inerno.Core.Features.Products.Commands.DeleteProductCommand;
using E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductCommand;
using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ProductControl.Commands
{
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [Route("api/Product")]
    public class ProductCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCommandsController(IMediator mediator)
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

        [HttpDelete("{ProductID}")]
        public async Task<Result<bool>> DeleteProduct(Guid ProductID)
        {
            return await _mediator.Send(new DeleteProductCmd(ProductID));
        }
    }
}
