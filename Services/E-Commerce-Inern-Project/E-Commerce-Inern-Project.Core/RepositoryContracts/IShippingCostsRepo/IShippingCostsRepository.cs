using E_Commerce_Inern_Project.Core.Domain.Entity;

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.IShippingCostsRepo
{
    public interface IShippingCostsRepository
    {
        public Task<IEnumerable<ShippingCosts>> GetAllShippingCosts();
        public Task<ShippingCosts?> CreateNewShippingCosts(ShippingCosts NewShippingCosts);
        public Task<ShippingCosts?> GetShippingCosts(Guid ShippingCostID);
        public Task<ShippingCosts?> GetShippingCostDetailsByAreaID(Guid AreaID);
        public Task<bool> SaveChanges();
    }
}
