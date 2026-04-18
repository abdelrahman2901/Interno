using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd
{
    public class ColorRequest : IRequest<Result<ColorResponse>>
    {
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
    }
}
