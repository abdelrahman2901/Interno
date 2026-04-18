using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.GetAllProductsDetailsQ
{

    public record GetAllProductDetailsQuery()
        : IRequest<Result<IEnumerable<ProductDetails_vw>>>;
}
