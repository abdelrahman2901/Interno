using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.CreateShippingCostCmd;
using E_Commerce_Inern_Project.Core.Features.ShippingCosts.Command.UpdateShippingCostCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.IShippingCostsServices
{
    public interface IShippingCostsService
    {
        public Task<Result<IEnumerable<ShippingCostDetails>>> GetAllShippingCostDetails();
        public Task<Result<ShippingCostDetails>> GetShippingCostDetailsByAreaID(Guid AreaID);
        public Task<Result<bool>> CreateNewShippingCost(CreateShippingCostRequest NewShippingCost);
        public Task<Result<bool>> UpdateShippingCost(UpdateShippingCostRequest NewShippingCost);
        public Task<Result<bool>> DeleteShippingCost(Guid ShippingCostID);
    }
}
