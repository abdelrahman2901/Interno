namespace E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO
{
    public record ProductRateRequest(Guid ProductID,Guid UserID,double Rating);
}
