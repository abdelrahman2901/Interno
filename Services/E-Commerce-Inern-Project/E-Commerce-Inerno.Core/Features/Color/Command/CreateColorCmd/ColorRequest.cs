using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd
{
    public class ColorRequest : IRequest<Result<ColorResponse>>
    {
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
    }
}
