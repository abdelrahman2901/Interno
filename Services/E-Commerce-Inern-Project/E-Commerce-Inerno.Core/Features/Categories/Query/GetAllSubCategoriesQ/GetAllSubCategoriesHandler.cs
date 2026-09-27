using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Categories.Query.GetAllSubCategoriesQ
{
    public class GetAllSubCategoriesHandler : IRequestHandler<GetAllSubCategoriesQuery, Result<IEnumerable<SubCategoryResponse>>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly IDistributedCache _cache;

        public GetAllSubCategoriesHandler(ICategoryRepository catRepo, IDistributedCache cache, IMapper mapper)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<SubCategoryResponse>>> Handle(GetAllSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            string CatsKey = "Categories";
            string? cachedCats = await _cache.GetStringAsync(CatsKey);
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
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(CatsKey, CatsJson, options);

            return Result<IEnumerable<SubCategoryResponse>>.Success(Categories.Select(s => _Mapper.Map<SubCategoryResponse>(s)));
        }
    }
}
