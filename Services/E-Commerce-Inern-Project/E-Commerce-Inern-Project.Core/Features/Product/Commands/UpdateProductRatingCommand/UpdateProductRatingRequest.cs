using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.UpdateProductRatingCommand
{
    public record UpdateProductRatingRequest(Guid ProductID,double Rating) : IRequest<Result<bool>>;
    
}
