using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Command.CreateProductRateCmd
{
    public record CreateProductRateRequest(IEnumerable<ProductRateRequest> rates) : IRequest<Result<bool>>;
}