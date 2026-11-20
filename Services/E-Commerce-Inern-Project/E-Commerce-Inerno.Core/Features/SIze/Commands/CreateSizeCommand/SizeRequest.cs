using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.SizeDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.SIze.Commands.CreateSizeCommand
{
    public record SizeRequest(string SizeName) : IRequest<Result<bool>>;
    
}
