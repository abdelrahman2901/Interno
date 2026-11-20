using E_Commerce_Interno.Domain.Entity;

namespace E_Commerce_Inerno.Core.RepositoryContracts.IShippingCostsRepo
{
    public interface IShippingCostsRepository
    {
        public Task<IEnumerable<ShippingCosts>> GetAllShippingCosts();
        public Task<ShippingCosts?> GetShippingCosts(Guid ShippingCostID);
        public Task<ShippingCosts?> GetShippingCostDetailsByAreaID(Guid AreaID);
        public Task<bool> CreateNewShippingCosts(ShippingCosts NewShippingCosts);
        public Task<bool> SaveChanges();
    }
}
