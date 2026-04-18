using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo;
using E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductRatesServices;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Command.CreateProductRateCmd
{
    public class CreateProductRateHandler : IRequestHandler<CreateProductRateRequest, Result<bool>>
    {
        private readonly IProductRatesService _productRatesService;
        private readonly IProductService _ProductService;
        private readonly ITransectionRepository _transection;
        public CreateProductRateHandler(IProductRatesService productRatesService, ITransectionRepository transection ,IProductService ProductService)
        {
            _productRatesService = productRatesService;
            _ProductService = ProductService;
            _transection = transection;
        }
        public async Task<Result<bool>> Handle(CreateProductRateRequest request, CancellationToken cancellationToken)
        {
                return  await _productRatesService.CreateProductRateList(request.rates);
        }
    }
}
