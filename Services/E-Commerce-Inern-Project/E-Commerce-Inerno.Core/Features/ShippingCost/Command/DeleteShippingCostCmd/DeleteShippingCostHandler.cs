using E_Commerce_Inerno.Core.RepositoryContracts.IShippingCostsRepo;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace E_Commerce_Inerno.Core.Features.ShippingCost.Command.DeleteShippingCostCmd
{
    public class DeleteShippingCostHandler : IRequestHandler<DeleteShippingCostRequest, Result<bool>>
    {
        private readonly IShippingCostsRepository _ShippingCostRepository;
        private readonly IDistributedCache _cache;

        public DeleteShippingCostHandler(IShippingCostsRepository ShippingCostRepository, IDistributedCache cache)
        {
            _ShippingCostRepository = ShippingCostRepository;
            _cache = cache;
        }

        public async Task<Result<bool>> Handle(DeleteShippingCostRequest request, CancellationToken cancellationToken)
        {
            ShippingCosts? shippingCost = await _ShippingCostRepository.GetShippingCosts(request.ShippingCostID);
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
    }
}
