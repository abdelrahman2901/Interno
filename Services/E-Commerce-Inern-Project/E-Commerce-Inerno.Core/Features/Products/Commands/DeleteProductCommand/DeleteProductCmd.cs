using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Products.Commands.DeleteProductCommand
{
    public record DeleteProductCmd(Guid ProductID) : IRequest<Result<bool>>;
}
