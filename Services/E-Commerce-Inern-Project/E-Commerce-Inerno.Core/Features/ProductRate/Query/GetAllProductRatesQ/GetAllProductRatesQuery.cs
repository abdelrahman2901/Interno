using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRate.Query.GetAllProductRatesQ
{
    public record GetAllProductRatesQuery() : IRequest<Result<IEnumerable<ProductRateResponse>>>;
}
