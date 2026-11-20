using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Query.GetAllProductsDetailsQ
{

    public record GetAllProductDetailsQuery()
        : IRequest<Result<IEnumerable<ProductDetails_vw>>>;
}
