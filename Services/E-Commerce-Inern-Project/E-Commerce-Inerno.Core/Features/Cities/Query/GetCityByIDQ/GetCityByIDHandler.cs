using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Cities.Query.GetCityByIDQ
{
    public class GetCityByIDHandler : IRequestHandler<GetCityByIDQuery, Result<CityResponse>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetCityByIDHandler(ICityRepository cityRepository, IDistributedCache cache, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<CityResponse>> Handle(GetCityByIDQuery request, CancellationToken cancellationToken)
        {
            string CityKey = $"City:{request.CityID}";
            string? CachedCity = await _cache.GetStringAsync(CityKey);
            if (CachedCity != null)
            {
                CityResponse CityResponse = JsonSerializer.Deserialize<CityResponse>(CachedCity);
                return Result<CityResponse>.Success(CityResponse);
            }

            City? city = await _cityRepository.GetCity(request.CityID, false);
            if (city == null)
            {
                return Result<CityResponse>.NotFound("City with the given id does not exist.");
            }

            string jsonCity = JsonSerializer.Serialize<CityResponse>(_mapper.Map<CityResponse>(city));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(CityKey, jsonCity, options);

            return Result<CityResponse>.Success(_mapper.Map<CityResponse>(city));
        }
    }
}
