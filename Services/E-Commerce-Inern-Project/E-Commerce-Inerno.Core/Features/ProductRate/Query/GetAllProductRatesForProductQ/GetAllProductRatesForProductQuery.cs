using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ProductRate.Query.GetAllProductRatesForProductQ
{
    public record GetAllProductRatesForProductQuery(Guid ProductID) : IRequest<Result<IEnumerable<ProductRateResponse>>>;
}
