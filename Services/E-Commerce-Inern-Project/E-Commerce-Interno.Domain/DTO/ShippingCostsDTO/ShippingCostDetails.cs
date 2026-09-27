using E_Commerce_Interno.Domain.DTO.AreaDTO;

namespace E_Commerce_Interno.Domain.DTO.ShippingCostsDTO
{
    public class ShippingCostDetails
    {
        public Guid ShippingCostID { get; set; }
        public decimal ShippingCost { get; set; }  
        public AreaResponse Area { get; set; }
        public bool IsDeleted { get; set; }
    }
}
