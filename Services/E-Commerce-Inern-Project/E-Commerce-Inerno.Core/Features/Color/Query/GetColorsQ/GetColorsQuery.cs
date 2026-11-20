using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Color.Query.GetColorsQ
{
    public record GetColorsQuery() : IRequest<Result<IEnumerable<ColorResponse>>>;
}
