using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.Categories.Query.GetAllSubCategoriesQ
{
    public class GetAllSubCategoriesHandler : IRequestHandler<GetAllSubCategoriesQuery, Result<IEnumerable<SubCategoryResponse>>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly IDistributedCache _cache;
        private readonly string _cacheKey = "AllSubCategories";
        private readonly DistributedCacheEntryOptions _options;
        public GetAllSubCategoriesHandler(ICategoryRepository catRepo, IDistributedCache cache, IMapper mapper)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _cache = cache;
            _options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
        }

        public async Task<Result<IEnumerable<SubCategoryResponse>>> Handle(GetAllSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            string? cachedCats = await _cache.GetStringAsync(_cacheKey);
            if (cachedCats != null)
            {
                var cachedCategories = JsonSerializer.Deserialize<IEnumerable<SubCategoryResponse>>(cachedCats);
                return Result<IEnumerable<SubCategoryResponse>>.Success(cachedCategories);
            }

            var Categories = await _CatRepo.GetCategoriesWithSubCat();
            if (!Categories.Any())
            {
                return Result<IEnumerable<SubCategoryResponse>>.NotFound("No Categories Was Found");
            }

            string CatsJson = JsonSerializer.Serialize(Categories.Select(s => _Mapper.Map<SubCategoryResponse>(s)));

            await _cache.SetStringAsync(_cacheKey, CatsJson, _options);

            return Result<IEnumerable<SubCategoryResponse>>.Success(Categories.Select(s => _Mapper.Map<SubCategoryResponse>(s)));
        }
    }
}
