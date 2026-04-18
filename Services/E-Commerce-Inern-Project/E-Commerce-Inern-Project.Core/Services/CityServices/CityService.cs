using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ICityRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.Features.City.Commands.CreateCityCmd;
using E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.CityServices
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        public CityService(ICityRepository cityRepository, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _UserRepo = UserRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<bool>> AddCity(CityRequest City)
        {
            if (await _cityRepository.IsCityExistsByName(City.CityName))
            {
                return Result<bool>.BadRequest("City with the same name already exists.");
            }
            City? addedCity = await _cityRepository.AddCity(_mapper.Map<City>(City));
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to add city.");
            }

            string JsonNewValues=JsonSerializer.Serialize<City>(addedCity);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(City), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            string CityKey = $"City:{addedCity.CityID}";
            string CityJson = JsonSerializer.Serialize<CityResponse>(_mapper.Map<CityResponse>(addedCity));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            await _cache.SetStringAsync(CityKey, CityJson, options);
            return Result<bool>.Success(addedCity.IsDeleted);
        }

        public async Task<Result<bool>> DeleteCity(Guid id)
        {
            City? city = await _cityRepository.GetCity_Tracking(id);
            if (city == null)
            {
                return Result<bool>.NotFound("City with the given id does not exist.");
            }
            string JsonOldValues = JsonSerializer.Serialize<City>(city);
            city.IsDeleted = true;
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete city.");
            }

            string JsonNewValues = JsonSerializer.Serialize<City>(city);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(City),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            string CityKey = $"City:{city.CityID}";
            await _cache.RemoveAsync(CityKey);
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<CityResponse>>> GetAllCities()
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

        public async Task<Result<CityResponse>> GetCity(Guid id)
        {

            string CityKey = $"City:{id}";
            string? CachedCity = await _cache.GetStringAsync(CityKey);
            if (CachedCity != null)
            {
                CityResponse CityResponse = JsonSerializer.Deserialize<CityResponse>(CachedCity);
                return Result<CityResponse>.Success(CityResponse);
            }

            City? city = await _cityRepository.GetCity_NoTracking(id);
            if (city == null)
            {
                return Result<CityResponse>.NotFound("City with the given id does not exist.");
            }

            string jsonCity = JsonSerializer.Serialize<CityResponse>(_mapper.Map<CityResponse>(city));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(CityKey, jsonCity, options);

            return Result<CityResponse>.Success(_mapper.Map<CityResponse>(city));
        }

        public async Task<Result<bool>> UpdateCity(CityUpdateRequest UpdateCityReq)
        {
            City? city = await _cityRepository.GetCity_Tracking(UpdateCityReq.CityID);
            if (city == null)
            {
                return Result<bool>.NotFound("City with the given id does not exist.");
            }

            string JsonOldValues = JsonSerializer.Serialize<City>(city);

            _mapper.Map(UpdateCityReq, city);
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to update city.");
            }

            string JsonNewValues = JsonSerializer.Serialize<City>(city);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(City), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            string CityKey = $"City:{city.CityID}";
            await _cache.RemoveAsync(CityKey);
            return Result<bool>.Success(city.IsDeleted);
        }


        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com");
            return admin.Id;
        }

    }
}
