using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.CreateProductCommand
{
    public class CreateProductHandler : IRequestHandler<ProductRequest, Result<bool>>
    {
        private readonly IProductService _productService;
        public CreateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Result<bool>> Handle(ProductRequest request,CancellationToken cancellationToken)
        {
            return await _productService.CreateProductAsync(request);
        }
    }
}
