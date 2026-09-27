using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRate.Command.DeleteProductRateCmd
{
    public record DeleteProductRateRequest(Guid RateID) : IRequest<Result<bool>>;
}
