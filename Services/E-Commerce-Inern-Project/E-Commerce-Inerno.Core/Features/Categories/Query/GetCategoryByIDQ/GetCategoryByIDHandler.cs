using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICategoryRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CategoryDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Categories.Query.GetCategoryByIDQ
{
    public class GetCategoryByIDHandler : IRequestHandler<GetCategoryByIDQuery, Result<CategoryResponse>>
    {
        private readonly ICategoryRepository _CatRepo;
        private readonly IMapper _Mapper;
        private readonly IDistributedCache _cache;

        public GetCategoryByIDHandler(ICategoryRepository catRepo, IDistributedCache cache, IMapper mapper)
        {
            _CatRepo = catRepo;
            _Mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<CategoryResponse>> Handle(GetCategoryByIDQuery request, CancellationToken cancellationToken)
        {
            string CatKey = $"Category:{request.CatID}";
            string? CachedCat = await _cache.GetStringAsync(CatKey);
            if (CachedCat != null)
            {
                var cachedCategory = JsonSerializer.Deserialize<CategoryResponse>(CachedCat);
                return Result<CategoryResponse>.Success(cachedCategory);
            }

            var Category = await _CatRepo.GetCategoryById(request.CatID);
            if (Category == null)
            {
                return Result<CategoryResponse>.NotFound("No Categories Was Found");
            }

            var response = _Mapper.Map<CategoryResponse>(Category);

            string CatJson = JsonSerializer.Serialize<CategoryResponse>(_Mapper.Map<CategoryResponse>(Category));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await _cache.SetStringAsync(CatKey, CatJson, options);

            return Result<CategoryResponse>.Success(response);
        }
    }
}
