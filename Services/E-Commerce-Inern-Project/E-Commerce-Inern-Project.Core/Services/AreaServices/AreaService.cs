
using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IAreaRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.UpdateAreaCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.AreaServices
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";


        public AreaService(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _cache = cache;
        }

        public async Task<Result<AreaResponse>> AddArea(AreaRequest area)
        {
            if (await _areaRepository.IsAreaExistsByName(area.AreaName))
            {
                return Result<AreaResponse>.BadRequest("Area with the same name already exists.");
            }
            var newArea = _mapper.Map<Area>(area);
            bool addedArea = await _areaRepository.AddArea(newArea);
            if (!addedArea)
            {
                return Result<AreaResponse>.InternalError("Failed to add Area.");
            }

            if (!await _areaRepository.SaveChanges())
            {
                return Result<AreaResponse>.InternalError("Failed to Save Changes.");
            }

            string jsonNewValues=JsonSerializer.Serialize<Area>(newArea);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(Area), null, jsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //Inserting value to Caching
            string AreaKey = $"Area:{newArea.AreaID}";
            var json = JsonSerializer.Serialize(_mapper.Map<AreaResponse>(newArea));
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(AreaKey, json, options);

            return Result<AreaResponse>.Success(_mapper.Map<AreaResponse>(newArea));
        }

        public async Task<Result<bool>> DeleteArea(Guid id)
        {
            Area? Area = await _areaRepository.GetArea_Tracking(id);
            if (Area == null)
            {
                return Result<bool>.NotFound("Area with the given id does not exist.");
            }
            Area.IsDeleted = true;
            if (!await _areaRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete Area.");
            }
            string json = JsonSerializer.Serialize<Area>(Area);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(Area), json, null);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //removing value from Caching
            await _cache.RemoveAsync($"Area:{id}");
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<AreaResponse>>> GetAllAreas()
        {

            string AreasKey = "Areas";

            string? areasCache = await _cache.GetStringAsync(AreasKey);
            if (areasCache != null)
            {
                var areas = JsonSerializer.Deserialize<IEnumerable<AreaResponse>>(areasCache);
                return Result<IEnumerable<AreaResponse>>.Success(areas);
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

        public async Task<Result<AreaResponse>> GetArea(Guid id)
        {

            string AreaKey = $"Area:{id}";

            string? areaCache = await _cache.GetStringAsync(AreaKey);
            if (areaCache != null)
            {
                var area = JsonSerializer.Deserialize<AreaResponse>(areaCache);
                return Result<AreaResponse>.Success(area);
            }


            Area? Area = await _areaRepository.GetArea_NoTracking(id);
            if (Area == null)
            {
                return Result<AreaResponse>.NotFound("Area with the given id does not exist.");
            }

            //Caching
            var json = JsonSerializer.Serialize(_mapper.Map<AreaResponse>(Area));
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(AreaKey, json, options);
            return Result<AreaResponse>.Success(_mapper.Map<AreaResponse>(Area));
        }

        public async Task<Result<bool>> UpdateArea(AreaUpdateRequest UpdatedArea)
        {
            Area? Area = await _areaRepository.GetArea_Tracking(UpdatedArea.AreaID);
            if (Area == null)
            {
                return Result<bool>.NotFound("Area with the given id does not exist.");
            }
            _mapper.Map(UpdatedArea, Area);
            if (!await _areaRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to update Area.");
            }

            string jsonOldValue = JsonSerializer.Serialize<Area>(Area);
            _mapper.Map(UpdatedArea, Area);
            string jsonNewValue = JsonSerializer.Serialize<Area>(Area);

            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Update, nameof(Area), jsonOldValue, jsonNewValue);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //Removing value from Caching
            await _cache.RemoveAsync($"Area:{UpdatedArea.AreaID}");

            return Result<bool>.Success(Area.IsDeleted);
        }

        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com");
            return admin.Id;
        }
    }
}
