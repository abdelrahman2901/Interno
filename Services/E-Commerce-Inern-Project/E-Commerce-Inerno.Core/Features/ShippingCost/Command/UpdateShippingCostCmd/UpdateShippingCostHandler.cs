using AutoMapper;
using E_Commerce_Inern_Project.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace E_Commerce_Inern_Project.Core.Features.ShippingCost.Command.UpdateShippingCostCmd
{
    public class UpdateShippingCostHandler : IRequestHandler<UpdateShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UpdateShippingCostHandler(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache, IMapper mapper)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(UpdateShippingCostRequest request, CancellationToken cancellationToken)
        {
            ShippingCosts? shippingCost = await _ShippingCostRepository.GetShippingCosts(request.ShippingCostID);
            if (shippingCost == null)
            {
                return Result<bool>.NotFound("ShippingCost Wasnt Found");
            }
            _mapper.Map(request, shippingCost);
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
