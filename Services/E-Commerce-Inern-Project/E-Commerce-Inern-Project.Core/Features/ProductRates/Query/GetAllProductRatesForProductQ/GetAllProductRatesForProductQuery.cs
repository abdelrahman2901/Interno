using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using MediatR;
 

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetAllProductRatesForProductQ
{
    public record GetAllProductRatesForProductQuery(Guid ProductID) : IRequest<Result<IEnumerable<ProductRateResponse>>>;
   
}
