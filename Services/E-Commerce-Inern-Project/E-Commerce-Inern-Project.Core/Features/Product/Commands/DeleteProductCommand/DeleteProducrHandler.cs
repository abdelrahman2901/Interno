using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.DeleteProductCommand
{
    internal class DeleteProducrHandler : IRequestHandler<DeleteProductCmd, Result<bool>>
    {
        private readonly IProductService _ProductService;
        public DeleteProducrHandler(IProductService productService)
        {
            _ProductService = productService;
        }

        public async Task<Result<bool>> Handle(DeleteProductCmd request, CancellationToken cancellationToken)
        {
            return await _ProductService.DeleteProductAsync(request.ProductID);
        }
    }
}
