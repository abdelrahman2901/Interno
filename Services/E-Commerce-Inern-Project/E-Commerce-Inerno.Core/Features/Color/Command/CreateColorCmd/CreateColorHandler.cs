using AutoMapper;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IUserRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd
{
    public class CreateColorHandler : IRequestHandler<ColorRequest, Result<ColorResponse>>
    {
        private readonly IColorRepository _colorRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        private readonly string _AuditRoutingKey = "Interno.Audit";

        public CreateColorHandler(IColorRepository colorRepository, IDistributedCache cache, IUserRepository UserRepo, IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _colorRepo = colorRepository;
            _UserRepo = UserRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _cache = cache;
        }

        public async Task<Result<ColorResponse>> Handle(ColorRequest request, CancellationToken cancellationToken)
        {
            Colors color = _mapper.Map<Colors>(request);
            bool result = await _colorRepo.CreateColors(color);
            if (!result)
            {
                return Result<ColorResponse>.InternalError("Failed to create color.");
            }

            string JsonNewValues = JsonSerializer.Serialize<Colors>(color);
            AuditRequest AuditRequest = new(await _UserRepo.GetAdminID(), ActionTypeEnum.Create, nameof(Colors), null, JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);

            //Inserting Data To Cahce
            string ColorKey = $"Color:{color.ColorID}";
            string ColorJson = JsonSerializer.Serialize<ColorResponse>(_mapper.Map<ColorResponse>(color));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)).SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(ColorKey, ColorJson, options);

            return Result<ColorResponse>.Success(_mapper.Map<ColorResponse>(color));
        }
    }
}
