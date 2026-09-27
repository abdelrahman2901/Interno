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

namespace E_Commerce_Inern_Project.Core.Features.Areas.Commands.DeleteAreaCmd
{
    public class DeleteAreaHandler : IRequestHandler<DeleteAreaCmd, Result<bool>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IUserRepository _UserRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteAreaHandler(IAreaRepository areaRepository, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
        {
            _areaRepository = areaRepository;
            _Publisher = Publisher;
            _UserRepo = UserRepo;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteAreaCmd request, CancellationToken cancellationToken)
        {
            Area? area = await _areaRepository.GetArea(request.AreaID, true);
            if (area == null)
            {
                return Result<bool>.NotFound("Area with the given id does not exist.");
            }
            area.IsDeleted = true;
            if (!await _areaRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete Area.");
            }

            string json = JsonSerializer.Serialize<Area>(area);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(Area), json, null);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //removing value from Caching
            await _cache.RemoveAsync($"Area:{request.AreaID}");
            return Result<bool>.Success(true);
        }
    }
}
