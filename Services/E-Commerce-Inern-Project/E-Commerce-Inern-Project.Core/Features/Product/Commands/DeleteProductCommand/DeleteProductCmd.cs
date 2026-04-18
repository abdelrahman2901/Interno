using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Product.Commands.DeleteProductCommand
{
    public record DeleteProductCmd(Guid ProductID) : IRequest<Result<bool>>;
}
