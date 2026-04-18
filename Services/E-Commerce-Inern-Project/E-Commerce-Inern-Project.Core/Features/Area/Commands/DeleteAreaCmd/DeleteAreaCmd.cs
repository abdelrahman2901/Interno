using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.DeleteAreaCmd
{
    public record DeleteAreaCmd(Guid AreaID) : IRequest<Result<bool>>;
}
