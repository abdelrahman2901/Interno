using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.DeleteColorCmd
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorRequest, Result<bool>>
    {
        private readonly IColorRepository _colorRepo;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public DeleteColorHandler(IColorRepository colorRepository, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher)
        {
            _colorRepo = colorRepository;
            _UserRepo = UserRepo;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteColorRequest request, CancellationToken cancellationToken)
        {
            Colors? color = await _colorRepo.GetColor(request.ColorID);
            if (color == null)
            {
                return Result<bool>.NotFound("Color not found.");
            }
            string JsonOldValues = JsonSerializer.Serialize<Colors>(color);
            color.IsDeleted = true;
            if (!await _colorRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to delete color.");
            }
            string JsonNewValues = JsonSerializer.Serialize<Colors>(color);

            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Delete, nameof(Colors), JsonOldValues, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //Removing Data From Cahce
            string ColorKey = $"Color:{color.ColorID}";
            await _cache.RemoveAsync(ColorKey);

            return Result<bool>.Success(true);
        }
    }
}
