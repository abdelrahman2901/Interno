using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Domain.ViewEntites;
 
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Query.GetProductByIDQ
{
    public record GetProductByIdQuery(Guid ProductID) : IRequest<Result<ProductDetails_vw>>;
}
