using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.DeleteColorCmd
{
    public record DeleteColorRequest(Guid ColorID) : IRequest<Result<bool>>;

}
