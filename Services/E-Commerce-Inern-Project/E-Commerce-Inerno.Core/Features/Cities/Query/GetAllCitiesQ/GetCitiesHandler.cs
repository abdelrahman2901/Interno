using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.Cities.Query.GetAllCitiesQ
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, Result<IEnumerable<CityResponse>>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetCitiesHandler(ICityRepository cityRepository, IDistributedCache cache, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<CityResponse>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            string CitiesKey = "Cities";
            string? CachedCities = await _cache.GetStringAsync(CitiesKey);
            if (CachedCities != null)
            {
                var CachedCitiesResponse = JsonSerializer.Deserialize<IEnumerable<CityResponse>>(CachedCities);
                return Result<IEnumerable<CityResponse>>.Success(CachedCitiesResponse);
            }

            IEnumerable<City> cities = await _cityRepository.GetAllCities();
            if (!cities.Any())
            {
                return Result<IEnumerable<CityResponse>>.NotFound("No cities found.");
            }

            string jsonCity = JsonSerializer.Serialize<IEnumerable<CityResponse>>(cities.Select(s => _mapper.Map<CityResponse>(s)));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(CitiesKey, jsonCity, options);

            return Result<IEnumerable<CityResponse>>.Success(cities.Select(c => _mapper.Map<CityResponse>(c)));
        }
    }
}
