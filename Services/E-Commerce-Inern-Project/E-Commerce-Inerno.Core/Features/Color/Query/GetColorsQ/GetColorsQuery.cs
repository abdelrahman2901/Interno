using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ
{
    public record GetColorsQuery() : IRequest<Result<IEnumerable<ColorResponse>>>;
}
