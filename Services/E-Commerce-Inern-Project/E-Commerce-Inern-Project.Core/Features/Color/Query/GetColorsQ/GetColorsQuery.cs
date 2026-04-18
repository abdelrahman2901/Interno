using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ
{
    public record GetColorsQuery() : IRequest<Result<IEnumerable<ColorResponse>>>;
}
