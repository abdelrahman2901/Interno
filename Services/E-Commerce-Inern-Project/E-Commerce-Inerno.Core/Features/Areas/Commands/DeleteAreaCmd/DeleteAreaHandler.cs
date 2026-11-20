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

namespace E_Commerce_Inerno.Core.Features.Areas.Commands.DeleteAreaCmd
{
    public class DeleteAreaHandler : IRequestHandler<DeleteAreaCmd, Result<bool>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IUserRepository _UserRepo;
        private readonly IInternoEventPublisher _Publisher;
        private readonly IDistributedCache _cache;
        

        public DeleteAreaHandler(IAreaRepository areaRepository, IDistributedCache cache, IUserRepository UserRepo, IInternoEventPublisher Publisher)
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
            await _Publisher.PublishAuditAsync(AuditRequest);

            //removing value from Caching
            await _cache.RemoveAsync($"Area:{request.AreaID}");
            return Result<bool>.Success(true);
        }
    }
}
