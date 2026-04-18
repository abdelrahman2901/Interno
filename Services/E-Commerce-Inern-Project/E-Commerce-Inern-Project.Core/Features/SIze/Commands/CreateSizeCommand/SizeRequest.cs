using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using MediatR;
 
namespace E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand
{
    public record SizeRequest(string SizeName) : IRequest<Result<SizeResponse>>;
    
}
