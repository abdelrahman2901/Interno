using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Commands.UpdateProductRatingCommand
{
    public record UpdateProductRatingRequest(Guid ProductID,double Rating) : IRequest<Result<bool>>;
    
}
