using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using E_Commerce_Inern_Project.Core.ServicesContracts.IProductServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.GetProductDetailsByIDQ
{
    public class GetProductDetailsByIDHandler : IRequestHandler<GetProductDetailsByIDQuery, Result<ProductDetails_vw>>
    {
        private readonly IProductViewService _ProductViewservice;
        public GetProductDetailsByIDHandler(IProductViewService productViewservice)
        {
            _ProductViewservice = productViewservice;
        }

        public async Task<Result<ProductDetails_vw>> Handle(GetProductDetailsByIDQuery request, CancellationToken cancellationToken)
        {
            return await _ProductViewservice.GetProductDetailsByID(request.ProductID);
        }
    }
}
