using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.ProductRate.Command.CreateProductRateCmd
{
    public record CreateProductRateRequest(IEnumerable<ProductRateRequest> rates) : IRequest<Result<bool>>;
}
