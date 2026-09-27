using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Products.Query.FilterProductsDetailsQ
{
    public record FilterProductsDetailsQuery( string? Category,string? SubCategory,decimal? Price,  string? Size,string? Color, string? Sort) : IRequest<Result<IEnumerable<ProductDetails_vw>>>;
}
