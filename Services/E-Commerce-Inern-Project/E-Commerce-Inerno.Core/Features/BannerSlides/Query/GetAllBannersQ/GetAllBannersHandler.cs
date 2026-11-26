using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IBannerRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.BannerSlideDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.BannerSlides.Query.GetAllBannersQ
{
    public class GetAllBannersHandler : IRequestHandler<GetAllBannersQuery, Result<IEnumerable<BannerSlideResponse>>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly string _cacheKey = "AllBanners";
        private readonly DistributedCacheEntryOptions _options;

        public GetAllBannersHandler(IBannerRepository bannerRepository, IMapper mapper, IDistributedCache cache)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _cache = cache;
            _options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromHours(1));

        }

        public async Task<Result<IEnumerable<BannerSlideResponse>>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
        {
            var cachedBanners = await _cache.GetStringAsync(_cacheKey);
            if(cachedBanners != null)
            {
                var banners = JsonSerializer.Deserialize<IEnumerable<BannerSlideResponse>>(cachedBanners);
                return Result<IEnumerable<BannerSlideResponse>>.Success(banners);
            }

            var result = await _bannerRepository.GetAllBanners();
            if (!result.Any())
            {
                return Result<IEnumerable<BannerSlideResponse>>.NotFound("No Banner Was Found.");
            }

            var json = JsonSerializer.Serialize(result.Select(s => _mapper.Map<BannerSlideResponse>(s)));
            await _cache.SetStringAsync(_cacheKey, json, _options);


            return Result<IEnumerable<BannerSlideResponse>>.Success(result.Select(s => _mapper.Map<BannerSlideResponse>(s)));
        }
    }
}
