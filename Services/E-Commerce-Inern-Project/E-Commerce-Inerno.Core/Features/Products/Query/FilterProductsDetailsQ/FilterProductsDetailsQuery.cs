using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductsDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ
{
    public record FilterProductsDetailsQuery(
        int PageNumber = 1,
        int PageSize = 20,
        Guid? CategoryID = null,
        Guid? SubCategoryID = null,
        Guid? SizeID = null,
        Guid? ColorID = null,
        decimal? Price = null,
        string? Sort = null) : IRequest<Result<GetProductsDto>>;
}
