using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.SIze.Commands.DeleteSizeCommand
{
    public record DeleteSizeCmd(Guid SizeID) : IRequest<Result<bool>>;
}
