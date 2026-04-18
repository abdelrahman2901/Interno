using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ISizeRepo;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand;
using E_Commerce_Inern_Project.Core.ServicesContracts.ISizeServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace E_Commerce_Inern_Project.Core.Services.SizeServices
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _SizeRepo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public SizeService(ISizeRepository sizeRepo, IDistributedCache cache, IMapper mapper)
        {
            _SizeRepo = sizeRepo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<SizeResponse?>> CreateSize(SizeRequest size)
        {
            Size? Newsize = await _SizeRepo.CreateSize(_mapper.Map<Size>(size));
            if (Newsize != null)
            {
                return Result<SizeResponse?>.InternalError("Failed to create size.");
            }
            if (!await _SizeRepo.SaveChanges())
            {
                return Result<SizeResponse?>.InternalError("Failed to save changes.");
            }

            //Inserting value to Caching
            string SizeKey = $"Size:{Newsize.SizeID}";
            var json = JsonSerializer.Serialize(_mapper.Map<SizeResponse>(Newsize));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(SizeKey, json, options);

            return Result<SizeResponse?>.Success(_mapper.Map<SizeResponse?>(Newsize));
        }

        public async Task<Result<bool>> DeleteSize(Guid sizeID)
        {
            var Size = await _SizeRepo.GetSize(sizeID);
            if (Size == null)
            {
                return Result<bool>.NotFound("No sizes found.");
            }
            Size.IsDeleted = true;

            if (!await _SizeRepo.SaveChanges())
            {
                return Result<bool>.InternalError("Failed to save changes.");
            }

            await _cache.RemoveAsync($"Size:{sizeID}");
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<SizeResponse>>> GetAllSizes()
        {

            //Inserting value to Caching
            string SizesKey = "Sizes";
            var CacheData = await _cache.GetStringAsync(SizesKey);
            if (CacheData != null)
            {
                var CachedSizes = JsonSerializer.Deserialize<IEnumerable<SizeResponse>>(CacheData);
                if (CachedSizes != null)
                {
                    return Result<IEnumerable<SizeResponse>>.Success(CachedSizes);
                }
            }




            var Sizes = await _SizeRepo.GetAllSizes();
            if (!Sizes.Any())
            {
                return Result<IEnumerable<SizeResponse>>.NotFound("No sizes found.");
            }

            var json = JsonSerializer.Serialize(Sizes.Select(s => _mapper.Map<SizeResponse>(s)));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(SizesKey, json, options);

            return Result<IEnumerable<SizeResponse>>.Success(_mapper.Map<IEnumerable<SizeResponse>>(Sizes));

        }


    }
}
