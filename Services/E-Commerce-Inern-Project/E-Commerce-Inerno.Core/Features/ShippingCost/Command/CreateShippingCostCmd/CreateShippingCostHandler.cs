using AutoMapper;
using E_Commerce_Inerno.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ShippingCostsDTO;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.CreateShippingCostCmd
{
    public class CreateShippingCostHandler : IRequestHandler<CreateShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CreateShippingCostHandler(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache, IMapper mapper)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(CreateShippingCostRequest request, CancellationToken cancellationToken)
        {
            ShippingCosts? shippingCost = _mapper.Map<ShippingCosts>(request);
            bool addingResult = await _ShippingCostRepository.CreateNewShippingCosts(shippingCost);
            if (!addingResult)
            {
                return Result<bool>.InternalError("Failed To Create Shipping cost");
            }

            if (!await _ShippingCostRepository.SaveChanges())
            {
                return Result<bool>.InternalError("Failed To Save Changes");
            }

            //Inserting into the cache
            string ShippingKey = $"ShippingCost:{shippingCost.ShippingCostID}";
            var json = JsonSerializer.Serialize<ShippingCostDetails>(_mapper.Map<ShippingCostDetails>(shippingCost));
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            await _cache.SetStringAsync(ShippingKey, json, options);

            return Result<bool>.Success(addingResult);
        }
    }
}
