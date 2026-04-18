using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductCommand
{
    internal class UpdateProductHandler : IRequestHandler<ProductUpdateRequest, Result<bool>>
    {
        private readonly IProductService _ProductService;
        public UpdateProductHandler(IProductService productService)
        {
            _ProductService = productService;
        }

        public async Task<Result<bool>> Handle(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            return await _ProductService.UpdateProductAsync(request);
        }
    }
}
