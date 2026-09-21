 

namespace E_Commerce_Inern_Project.Core.DTO.ShippingCostsDTO
{
    public class ShippingCostResponse
    {
        public Guid ShippingCostID { get; set; }
        public decimal ShippingCost { get; set; }  
        public Guid AraeID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
