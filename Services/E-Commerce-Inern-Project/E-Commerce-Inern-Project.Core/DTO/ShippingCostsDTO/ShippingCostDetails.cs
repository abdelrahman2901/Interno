using E_Commerce_Inern_Project.Core.Domain.Entity;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;

namespace E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO
{
    public class ShippingCostDetails
    {
        public Guid ShippingCostID { get; set; }
        public decimal ShippingCost { get; set; }  
        public AreaResponse Area { get; set; }
        public bool IsDeleted { get; set; }
    }
}
