using AutoMapper;
using E_Commerce_Inerno.Core.Communication;

using E_Commerce_Inerno.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Inerno.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO.Enum;
using E_Commerce_Inerno.Core.Communication.DTOs.AuditDTO;

namespace E_Commerce_Inerno.Core.Features.Areas.Commands.UpdateAreaCmd
{
    public class UpdateAreaHandler : IRequestHandler<AreaUpdateRequest, Result<bool>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IDistributedCache _cache;
        

        public UpdateAreaHandler(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper, IUserRepository UserRepo, IInternoEventPublisher Publisher)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(AreaUpdateRequest request, CancellationToken cancellationToken)
        {
            Area? area = await _areaRepository.GetArea(request.AreaID, true);
            if (area == null)
            {
                return Result<bool>.NotFound("Area with the given id does not exist.");
            }
            _mapper.Map(request, area);
            if (!await _areaRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to update Area.");
            }

            string jsonOldValue = JsonSerializer.Serialize<Area>(area);
            _mapper.Map(request, area);
            string jsonNewValue = JsonSerializer.Serialize<Area>(area);

            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Update, nameof(Area), jsonOldValue, jsonNewValue);
            await _Publisher.PublishAuditAsync(AuditRequest);

            //Removing value from Caching
            await _cache.RemoveAsync($"Area:{request.AreaID}");

            return Result<bool>.Success(area.IsDeleted);
        }
    }
}
