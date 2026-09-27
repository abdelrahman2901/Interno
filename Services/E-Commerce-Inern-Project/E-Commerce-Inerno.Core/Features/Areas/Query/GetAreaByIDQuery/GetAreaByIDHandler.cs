using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Query.GetAreaByIDQuery
{
    public class GetAreaByIDHandler : IRequestHandler<GetAreaByIDRequest, Result<AreaResponse>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetAreaByIDHandler(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<AreaResponse>> Handle(GetAreaByIDRequest request, CancellationToken cancellationToken)
        {
            string AreaKey = $"Area:{request.AreaID}";

            string? areaCache = await _cache.GetStringAsync(AreaKey);
            if (areaCache != null)
            {
                var cachedArea = JsonSerializer.Deserialize<AreaResponse>(areaCache);
                return Result<AreaResponse>.Success(cachedArea);
            }

            Area? area = await _areaRepository.GetArea(request.AreaID, false);
            if (area == null)
            {
                return Result<AreaResponse>.NotFound("Area with the given id does not exist.");
            }

            //Caching
            var json = JsonSerializer.Serialize(_mapper.Map<AreaResponse>(area));
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(AreaKey, json, options);
            return Result<AreaResponse>.Success(_mapper.Map<AreaResponse>(area));
        }
    }
}
