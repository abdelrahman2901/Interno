using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Products.Commands.UpdateProductRatingCommand
{
    public record UpdateProductRatingRequest(Guid ProductID,double Rating) : IRequest<Result<bool>>;
    
}
