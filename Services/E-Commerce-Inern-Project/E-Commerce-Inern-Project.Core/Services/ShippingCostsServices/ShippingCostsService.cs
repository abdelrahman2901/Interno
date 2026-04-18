using AutoMapper;
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd;
using E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Services.ShippingCostsServices
{
    public class ShippingCostsService : IShippingCostsService
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public ShippingCostsService(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache, IMapper mapper)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<bool>> CreateNewShippingCost(CreateShippingCostRequest NewShippingCost)
        {
            ShippingCosts? addingResult = await _ShippingCostRepository.CreateNewShippingCosts(_mapper.Map<ShippingCosts>(NewShippingCost));
            if (addingResult == null)
            {
                return Result<bool>.InternalError("Failed To Create Shipping cost");
            }

            if (!await _ShippingCostRepository.SaveChanges())
            {

                return Result<bool>.InternalError("Failed To Save Changes");
            }


            //Inserting into the cache
            string ShippingKey = $"ShippingCost:{addingResult.ShippingCostID}";
            var json = JsonSerializer.Serialize<ShippingCostDetails>(_mapper.Map<ShippingCostDetails>(addingResult));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            await _cache.SetStringAsync(ShippingKey, json, options);

            return Result<bool>.Success(addingResult != null);
        }

        public async Task<Result<bool>> DeleteShippingCost(Guid ShippingCostID)
        {
            ShippingCosts? shippingCost = await _ShippingCostRepository.GetShippingCosts(ShippingCostID);
            if (shippingCost == null)
            {
                return Result<bool>.NotFound("ShippingCost Wasnt Found");
            }
            shippingCost.IsDeleted = true;
            if (!await _ShippingCostRepository.SaveChanges())
            {

                return Result<bool>.InternalError("Failed To Save Changes");
            }

            //Removing from the cache
            string ShippingKey = $"ShippingCost:{shippingCost.ShippingCostID}";
            await _cache.RemoveAsync(ShippingKey);

            return Result<bool>.Success(shippingCost.IsDeleted);
        }

        public async Task<Result<IEnumerable<ShippingCostDetails>>> GetAllShippingCostDetails()
        {
            string ShippingsKey = "ShippingCosts";
            var ShippingsJson = await _cache.GetStringAsync(ShippingsKey);
            if (ShippingsJson != null)
            {
                var CachedShippingCosts = JsonSerializer.Deserialize<IEnumerable<ShippingCostDetails>>(ShippingsJson);

                return Result<IEnumerable<ShippingCostDetails>>.Success(CachedShippingCosts);
            }


            var shippingCosts = await _ShippingCostRepository.GetAllShippingCosts();
            if (!shippingCosts.Any())
            {
                return Result<IEnumerable<ShippingCostDetails>>.NotFound("no ShippingCosts Was Found");
            }


            //Inserting into the cache
            var json = JsonSerializer.Serialize<IEnumerable<ShippingCostDetails>>(shippingCosts.Select(s => _mapper.Map<ShippingCostDetails>(s)));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            await _cache.SetStringAsync(ShippingsKey, json, options);

            return Result<IEnumerable<ShippingCostDetails>>.Success(shippingCosts.Select(s => _mapper.Map<ShippingCostDetails>(s)));
        }

        public async Task<Result<ShippingCostDetails>> GetShippingCostDetailsByAreaID(Guid AreaID)
        {
            string ShippingKey = $"ShippingCostsByAreaID:{AreaID}";
            var ShippingJson = await _cache.GetStringAsync(ShippingKey);
            if (ShippingJson != null)
            {
                var CachedShippingCosts = JsonSerializer.Deserialize<ShippingCostDetails>(ShippingJson);

                return Result<ShippingCostDetails>.Success(CachedShippingCosts);
            }


            var shippingCost = await _ShippingCostRepository.GetShippingCostDetailsByAreaID(AreaID);
            if (shippingCost == null)
            {
                return Result<ShippingCostDetails>.NotFound("no ShippingCosts Was Found");
            }

            var json = JsonSerializer.Serialize<ShippingCostDetails>(_mapper.Map<ShippingCostDetails>(shippingCost));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            await _cache.SetStringAsync(ShippingKey, json, options);

            return Result<ShippingCostDetails>.Success(_mapper.Map<ShippingCostDetails>(shippingCost));
        }

        public async Task<Result<bool>> UpdateShippingCost(UpdateShippingCostRequest NewShippingCost)
        {

            ShippingCosts? shippingCost = await _ShippingCostRepository.GetShippingCosts(NewShippingCost.ShippingCostID);
            if (shippingCost == null)
            {
                return Result<bool>.NotFound("ShippingCost Wasnt Found");
            }
            _mapper.Map(NewShippingCost, shippingCost);
            if (!await _ShippingCostRepository.SaveChanges())
            {

                return Result<bool>.InternalError("Failed To Save Changes");
            }

            string ShippingKey = $"ShippingCost:{shippingCost.ShippingCostID}";
            string ShippingByAreaKey = $"ShippingCostsByAreaID:{shippingCost.AraeID}";
            await _cache.RemoveAsync(ShippingKey);
            //Incase the areaID was updated we need to remove the old cache entry for the areaID as well, and the new one will be created when requested.
            //Note: if theres no existing cache related with the area id nothing will happen.
            await _cache.RemoveAsync(ShippingByAreaKey);

            return Result<bool>.Success(shippingCost != null);
        }
    }
}
