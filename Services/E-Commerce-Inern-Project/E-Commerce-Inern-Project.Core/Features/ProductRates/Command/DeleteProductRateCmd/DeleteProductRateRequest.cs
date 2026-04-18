using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.ProductRates.Command.DeleteProductRateCmd
{
    public record DeleteProductRateRequest(Guid RateID) : IRequest<Result<bool>>;
}
