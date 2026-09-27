using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IAreaRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Commands.UpdateAreaCmd
{
    public class UpdateAreaHandler : IRequestHandler<AreaUpdateRequest, Result<bool>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public UpdateAreaHandler(IAreaRepository areaRepository, IDistributedCache cache, IMapper mapper, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
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
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //Removing value from Caching
            await _cache.RemoveAsync($"Area:{request.AreaID}");

            return Result<bool>.Success(area.IsDeleted);
        }
    }
}
