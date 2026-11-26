using E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ.Models;
using E_Commerce_Inerno.Core.RepositoryContracts.IProductRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductsDTO;
using E_Commerce_Interno.Domain.ViewEntites;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace E_Commerce_Inerno.Core.Features.Products.Query.FilterProductsDetailsQ
{
    public class FilterProductsDetailsHandler : IRequestHandler<FilterProductsDetailsQuery, Result<GetProductsDto>>
    {
        private readonly IProductViewRepository _productViewRepo;
        private readonly IDistributedCache _Cache;
        private readonly string _totalCountCacheKey = "ProductCount_{0}_{1}_{2}_{3}_{4}_{5}";
        private readonly string _queryCacheKey = "ProductQuery_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}";
        public FilterProductsDetailsHandler(IProductViewRepository productViewRepo, IDistributedCache Cache)
        {
            _productViewRepo = productViewRepo;
            _Cache = Cache;

        }

        public async Task<Result<GetProductsDto>> Handle(FilterProductsDetailsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
            var cachedQuery = await _Cache.GetAsync(string.Format(_queryCacheKey, request.CategoryID, request.SubCategoryID, request.SizeID, request.Sort, request.ColorID, request.Price, pageNumber, pageSize), cancellationToken);
            var cachedtotalCount = await _Cache.GetAsync(string.Format(_totalCountCacheKey, request.CategoryID, request.SubCategoryID, request.SizeID, request.Sort, request.ColorID, request.Price), cancellationToken);
            var totalCount = cachedtotalCount != null ? BitConverter.ToInt32(cachedtotalCount) : 0;

            if (cachedQuery != null)
            {
                var cachedProducts = System.Text.Json.JsonSerializer.Deserialize<List<ProductDetails_vw>>(cachedQuery);
                return Result<GetProductsDto>.Success(GetProductsDto.Create(cachedProducts, totalCount, pageNumber, pageSize));
            }

            (IEnumerable<ProductRaw> rawProducts, int total) = await _productViewRepo.FilterProductsQuery(request, pageNumber, pageSize, cancellationToken);
            totalCount = total;

            await _Cache.SetAsync(string.Format(_totalCountCacheKey, request.CategoryID, request.SubCategoryID, request.SizeID, request.Sort, request.ColorID, request.Price), BitConverter.GetBytes(totalCount), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }, cancellationToken);


            var products = rawProducts.Select(r => new ProductDetails_vw
            {
                ProductID = r.ProductID,
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
                ProductImageUrl = r.ProductImageUrl,
            }).ToList();

            return Result<GetProductsDto>.Success(GetProductsDto.Create(products, totalCount, pageNumber, pageSize));
        }
    }
}
