using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IColorServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd
{
    public class CreateColorHandler : IRequestHandler<ColorRequest, Result<ColorResponse>>
    {
        private readonly IColorService _colorService;
        public CreateColorHandler(IColorService colorService)
        {
            _colorService = colorService;
        }
        public async Task<Result<ColorResponse>> Handle(ColorRequest request, CancellationToken cancellationToken)
        {
            return await _colorService .CreateColor(request);
        }
    }
}
