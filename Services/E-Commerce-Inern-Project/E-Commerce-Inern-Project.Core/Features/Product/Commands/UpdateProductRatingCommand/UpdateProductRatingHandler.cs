using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand
{
    public class UpdateProductRatingHandler : IRequestHandler<UpdateProductRatingRequest, Result<bool>>
    {
        private readonly IProductService _productService;
        public UpdateProductRatingHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task<Result<bool>> Handle(UpdateProductRatingRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
