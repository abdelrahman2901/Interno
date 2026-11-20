using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Color.Command.DeleteColorCmd
{
    public record DeleteColorRequest(Guid ColorID) : IRequest<Result<bool>>;

}
