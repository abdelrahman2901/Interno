using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ
{
    public class GetColorsHandler : IRequestHandler<GetColorsQuery, Result<IEnumerable<ColorResponse>>>
    {
        private readonly IColorService _colorService;
        public GetColorsHandler(IColorService colorService)
        {
            _colorService = colorService;
        }

        public async Task<Result<IEnumerable<ColorResponse>>> Handle(GetColorsQuery request, CancellationToken cancellationToken)
        {
            return await _colorService.GetAllColors();
        }
    }
}
