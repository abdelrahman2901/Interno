using AutoMapper;
using E_Commerce_Inerno.Core.Communication;
using E_Commerce_Inerno.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Areas.Commands.CreateAreaCmd
{
    public class CreateAreaHandler : IRequestHandler<AreaRequest, Result<AreaResponse>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IDistributedCache _cache;
        

        public CreateAreaHandler(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper, IUserRepository UserRepo, IInternoEventPublisher Publisher)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _cache = cache;
        }

        public async Task<Result<AreaResponse>> Handle(AreaRequest request, CancellationToken cancellationToken)
        {
            if (await _areaRepository.IsAreaExistsByName(request.AreaName))
            {
                return Result<AreaResponse>.BadRequest("Area with the same name already exists.");
            }

            var newArea = _mapper.Map<Area>(request);
            bool addedArea = await _areaRepository.AddArea(newArea);
            if (!addedArea)
            {
                return Result<AreaResponse>.InternalError("Failed to add Area.");
            }

            if (!await _areaRepository.SaveChanges())
            {
                return Result<AreaResponse>.InternalError("Failed to Save Changes.");
            }

            string jsonNewValues = JsonSerializer.Serialize<Area>(newArea);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Create, nameof(Area), null, jsonNewValues);
            await _Publisher.PublishAuditAsync(AuditRequest);

            //Inserting value to Caching
            string AreaKey = $"Area:{newArea.AreaID}";
            var json = JsonSerializer.Serialize(_mapper.Map<AreaResponse>(newArea));
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            await _cache.SetStringAsync(AreaKey, json, options);

            return Result<AreaResponse>.Success(_mapper.Map<AreaResponse>(newArea));
        }
    }
}
