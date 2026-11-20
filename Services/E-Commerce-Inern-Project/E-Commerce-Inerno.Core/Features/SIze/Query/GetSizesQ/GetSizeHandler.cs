using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.ISizeRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.SizeDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.SIze.Query.GetSizesQ
{
    public class GetSizeHandler : IRequestHandler<GetSizesQuery, Result<IEnumerable<SizeResponse>>>
    {
        private readonly ISizeRepository _SizeRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetSizeHandler(ISizeRepository sizeRepo, IDistributedCache cache, IMapper mapper)
        {
            _SizeRepo = sizeRepo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<SizeResponse>>> Handle(GetSizesQuery request, CancellationToken cancellationToken)
        {
            //Inserting value to Caching
            string SizesKey = "Sizes";
            var CacheData = await _cache.GetStringAsync(SizesKey);
            if (CacheData != null)
            {
                var CachedSizes = JsonSerializer.Deserialize<IEnumerable<SizeResponse>>(CacheData);
                if (CachedSizes != null)
                {
                    return Result<IEnumerable<SizeResponse>>.Success(CachedSizes);
                }
            }

            var Sizes = await _SizeRepo.GetAllSizes();
            if (!Sizes.Any())
            {
                return Result<IEnumerable<SizeResponse>>.NotFound("No sizes found.");
            }

            var json = JsonSerializer.Serialize(Sizes.Select(s => _mapper.Map<SizeResponse>(s)));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(SizesKey, json, options);

            return Result<IEnumerable<SizeResponse>>.Success(_mapper.Map<IEnumerable<SizeResponse>>(Sizes));
        }
    }
}
