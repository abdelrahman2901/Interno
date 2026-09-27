using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Query.GetAllAreasQ
{
    public class GetAllAreasHandler : IRequestHandler<GetAllAreasQuery, Result<IEnumerable<AreaResponse>>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetAllAreasHandler(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<AreaResponse>>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
        {
            string AreasKey = "Areas";

            string? areasCache = await _cache.GetStringAsync(AreasKey);
            if (areasCache != null)
            {
                var cachedAreas = JsonSerializer.Deserialize<IEnumerable<AreaResponse>>(areasCache);
                return Result<IEnumerable<AreaResponse>>.Success(cachedAreas);
            }

            IEnumerable<Area> Areas = await _areaRepository.GetAllAreas();
            if (!Areas.Any())
            {
                return Result<IEnumerable<AreaResponse>>.NotFound("No cities found.");
            }

            //Caching
            var json = JsonSerializer.Serialize(Areas.Select(c => _mapper.Map<AreaResponse>(c)));
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            await _cache.SetStringAsync(AreasKey, json, options);
            return Result<IEnumerable<AreaResponse>>.Success(Areas.Select(c => _mapper.Map<AreaResponse>(c)));
        }
    }
}
