using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IColorRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ
{
    public class GetColorsHandler : IRequestHandler<GetColorsQuery, Result<IEnumerable<ColorResponse>>>
    {
        private readonly IColorRepository _colorRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetColorsHandler(IColorRepository colorRepository, IDistributedCache cache, IMapper mapper)
        {
            _colorRepo = colorRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<ColorResponse>>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
        {
            string ColorsKey = "Colors";
            string? CachedColors = await _cache.GetStringAsync(ColorsKey);
            if (CachedColors != null)
            {
                var CachedResponse = JsonSerializer.Deserialize<IEnumerable<ColorResponse>>(CachedColors);
                if (CachedResponse != null)
                {
                    return Result<IEnumerable<ColorResponse>>.Success(CachedResponse);
                }
            }

            var colors = await _colorRepo.GetAllColors();
            if (!colors.Any())
            {
                return Result<IEnumerable<ColorResponse>>.NotFound("No colors found.");
            }

            //Inserting Data To Cache   
            string ColorJson = JsonSerializer.Serialize<IEnumerable<ColorResponse>>(colors.Select(s => _mapper.Map<ColorResponse>(s)));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)).SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(ColorsKey, ColorJson, options);

            return Result<IEnumerable<ColorResponse>>.Success(colors.Select(s => _mapper.Map<ColorResponse>(s)));
        }
    }
}
