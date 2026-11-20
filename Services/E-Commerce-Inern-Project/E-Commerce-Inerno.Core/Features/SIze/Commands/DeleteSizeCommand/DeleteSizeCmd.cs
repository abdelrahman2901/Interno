using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.SIze.Commands.DeleteSizeCommand
{
    public record DeleteSizeCmd(Guid SizeID) : IRequest<Result<bool>>;
}
