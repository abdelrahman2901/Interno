using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.Product.Query.FilterProductsDetailsQ
{
    public record FilterProductsDetailsQuery( string? Category,string? SubCategory,decimal? Price,  string? Size,string? Color, string? Sort) : IRequest<Result<IEnumerable<ProductDetails_vw>>>;
}
