using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ProductRatesDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Query.GetUserRatingQ
{
    public record GetUserRatingQuery(Guid UserID) : IRequest<Result<IEnumerable<ProductRateResponse>>>;
}
