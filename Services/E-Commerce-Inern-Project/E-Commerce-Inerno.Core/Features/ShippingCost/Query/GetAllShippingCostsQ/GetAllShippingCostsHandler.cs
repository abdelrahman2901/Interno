using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCost.Query.GetAllShippingCostsQ
{
    public class GetAllShippingCostsHandler : IRequestHandler<GetAllShippingCostsRequest, Result<IEnumerable<ShippingCostDetails>>>
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetAllShippingCostsHandler(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache, IMapper mapper)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<ShippingCostDetails>>> Handle(GetAllShippingCostsRequest request, CancellationToken cancellationToken)
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
    }
}
