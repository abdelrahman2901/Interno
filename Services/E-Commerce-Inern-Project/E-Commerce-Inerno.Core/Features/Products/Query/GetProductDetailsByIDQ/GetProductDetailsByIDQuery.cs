using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Query.GetProductDetailsByIDQ
{
    public record GetProductDetailsByIDQuery(Guid ProductID) : IRequest<Result<ProductDetails_vw>>;
}
