using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Carts.Commands.CreateCartCmd
{
    public record CreateCartRequest(Guid UserID) : IRequest<Result<bool>>;
}
