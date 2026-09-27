namespace E_Commerce_Interno.Domain.DTO.ProductRatesDTO
{
    public record ProductRateRequest(Guid ProductID,Guid UserID,double Rating);
}
