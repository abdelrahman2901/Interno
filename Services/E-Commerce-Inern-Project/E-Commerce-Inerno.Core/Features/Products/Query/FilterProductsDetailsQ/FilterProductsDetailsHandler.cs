using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductsDTO;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ
{
    public class FilterProductsDetailsHandler : IRequestHandler<FilterProductsDetailsQuery, Result<GetProductsDto>>
    {
        private readonly IProductViewRepository _productViewRepo;

        public FilterProductsDetailsHandler(IProductViewRepository productViewRepo)
        {
            _productViewRepo = productViewRepo;
        }

        public async Task<Result<GetProductsDto>> Handle(FilterProductsDetailsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

            var query = _productViewRepo.FilterProductsQuery(request);
            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return Result<GetProductsDto>.NotFound("No Products Was Found");
            }

            var rawProducts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var products = rawProducts.Select(r => new ProductDetails_vw
            {
                ProductID = r.ProductID,
                CategoryID = r.CategoryID,
                ParentcategoryID = r.ParentcategoryID,
                SizeID = r.SizeID,
                ColorID = r.ColorID,
                CategoryName = r.CategoryName,
                ParentCategoryName = r.ParentCategoryName,
                ProductName = r.ProductName,
                Price = r.Price,
                SalePrice = r.SalePrice,
                Stock = r.Stock,
                TotalRating = r.RatingCount,
                Rating = r.RatingCount == 0
                    ? 0
                    : CalculateRatingCls.CalcuateTheRating(r.RatingCount, r.AvgRating),
                SizeName = r.SizeName,
                ColorName = r.ColorName,
                CreatedAt = r.CreatedAt,
                ProductImageUrl = r.ProductImageUrl,
                Active = r.Active,
                IsDeleted = r.IsDeleted
            }).ToList();

            return Result<GetProductsDto>.Success(GetProductsDto.Create(products, totalCount, pageNumber, pageSize));
        }
    }
}
