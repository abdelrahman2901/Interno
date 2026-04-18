using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IColorRepo;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IUserRepo;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd;
using E_Commerce_Inern_Project.Core.RabbitMQ;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO;
using E_Commerce_Inern_Project.Core.RabbitMQ.DTOs.AuditDTO.Enum;
using E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Drawing;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.ColorServices
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _Publisher;
        private readonly string _AuditRoutingKey = "Interno.Audit";
        private readonly IUserRepository _UserRepo;
        private readonly IDistributedCache _cache;
        public ColorService(IColorRepository colorRepository, IDistributedCache cache, IUserRepository UserRepo,IRabbitMQPublisher Publisher, IMapper mapper)
        {
            _colorRepo = colorRepository;
            _UserRepo = UserRepo;
            _mapper = mapper;
            _Publisher = Publisher;
            _cache = cache;
        }
        public async Task<Result<ColorResponse>> CreateColor(ColorRequest request)
        {
            Colors? color= await _colorRepo.CreateColors(_mapper.Map<Colors>(request));
            if(color == null)
            {
                return Result<ColorResponse>.InternalError("Failed to create color.");
            }

            string JsonNewValues= JsonSerializer.Serialize<Colors>(color);
            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Create, nameof(Colors),null,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            //Inserting Data To Cahce
            string ColorKey = $"Color:{color.ColorID}";
            string ColorJson= JsonSerializer.Serialize<ColorResponse>(_mapper.Map<ColorResponse>(color));
            var options= new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)).SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(ColorKey, ColorJson, options);

            return Result<ColorResponse>.Success(_mapper.Map<ColorResponse>(color));
        }

        public async Task<Result<bool>> DeleteColor(Guid ColorID)
        {
            Colors? color = await _colorRepo.GetColor(ColorID);
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

            AuditRequest AuditRequest = new(await GetAdminID(), ActionTypeEnum.Delete, nameof(Colors),JsonOldValues,JsonNewValues);
            await _Publisher.Publish(_AuditRoutingKey, AuditRequest);


            //Removing Data From Cahce
            string ColorKey = $"Color:{color.ColorID}";
            await _cache.RemoveAsync(ColorKey);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<ColorResponse>>> GetAllColors()
        {
            string ColorsKey = "Colors";
            string? CachedColors = await _cache.GetStringAsync(ColorsKey);
            if(CachedColors    != null)
            {
                var Colors = JsonSerializer.Deserialize<IEnumerable<ColorResponse>>(CachedColors);
                if(Colors != null)
                {
                    return Result<IEnumerable<ColorResponse>>.Success(Colors);
                }
            }

            var colors = await _colorRepo.GetAllColors();
            if (!colors.Any())
            {
                return Result<IEnumerable<ColorResponse>>.NotFound("No colors found.");
            }


            //Inserting Data To Cache   
            string ColorJson = JsonSerializer.Serialize<IEnumerable<ColorResponse>>(colors.Select(s=> _mapper.Map<ColorResponse>(s)));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)).SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(ColorsKey, ColorJson, options);


            return Result<IEnumerable<ColorResponse>>.Success(colors.Select(s => _mapper.Map<ColorResponse>(s)));
        }
        private async Task<Guid> GetAdminID()
        {
            var admin = await _UserRepo.GetApplicationUserByEmail("admin@gmail.com");
            return admin.Id;
        }

    }
}
