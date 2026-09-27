using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Commands.DeleteAreaCmd
{
    public record DeleteAreaCmd(Guid AreaID) : IRequest<Result<bool>>;
}
