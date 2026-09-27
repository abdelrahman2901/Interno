using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.ICityRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Cities.Commands.CreateCityCmd
{
    public class CityRequestHandler : IRequestHandler<CityRequest, Result<bool>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CityRequestHandler(ICityRepository cityRepository, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _UserRepo = UserRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(CityRequest request, CancellationToken cancellationToken)
        {
            if (await _cityRepository.IsCityExistsByName(request.CityName))
            {
                return Result<bool>.BadRequest("City with the same name already exists.");
            }
            City city = _mapper.Map<City>(request);
            bool result = await _cityRepository.AddCity(city);
            if (!result)
            {
                return Result<bool>.InternalError("Failed to add city.");
            }
            if (!await _cityRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to save Changes for city.");
            }

            string JsonNewValues = JsonSerializer.Serialize<City>(city);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(City), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            string CityKey = $"City:{city.CityID}";
            string CityJson = JsonSerializer.Serialize<CityResponse>(_mapper.Map<CityResponse>(city));
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            await _cache.SetStringAsync(CityKey, CityJson, options);
            return Result<bool>.Success(city.IsDeleted);
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com", false);
            return admin.UserID;
        }
    }
}
