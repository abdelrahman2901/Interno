using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Query.GetShippingCostDetailsByAreaIDQ
{
    public class GetShippingCostDetailsByAreaIDHandler : IRequestHandler<GetShippingCostDetailsByAreaIDRequest, Result<ShippingCostDetails>>
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetShippingCostDetailsByAreaIDHandler(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache, IMapper mapper)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<ShippingCostDetails>> Handle(GetShippingCostDetailsByAreaIDRequest request, CancellationToken cancellationToken)
        {
            string ShippingKey = $"ShippingCostsByAreaID:{request.AreaID}";
            var ShippingJson = await _cache.GetStringAsync(ShippingKey);
            if (ShippingJson != null)
            {
                var CachedShippingCosts = JsonSerializer.Deserialize<ShippingCostDetails>(ShippingJson);

                return Result<ShippingCostDetails>.Success(CachedShippingCosts);
            }

            var shippingCost = await _ShippingCostRepository.GetShippingCostDetailsByAreaID(request.AreaID);
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
    }
}
