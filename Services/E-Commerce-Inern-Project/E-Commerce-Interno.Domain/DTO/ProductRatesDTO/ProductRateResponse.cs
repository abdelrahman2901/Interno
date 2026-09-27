using E_Commerce_Interno.Domain.ViewEntites;

namespace E_Commerce_Interno.Domain.DTO.ProductRatesDTO
{
    public class ProductRateResponse
    {
            public Guid RateID { get; set; }
            public Guid UserID { get; set; }

            public ProductDetails_vw Product { get; set; }
            public double Rating { get; set; }
    }
}
