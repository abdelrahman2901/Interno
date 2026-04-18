using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd
{
    public class CreateAreaHandler : IRequestHandler<AreaRequest, Result<AreaResponse>>
    {
        private readonly IAreaService _areaService;
        public CreateAreaHandler(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<Result<AreaResponse>> Handle(AreaRequest request, CancellationToken cancellationToken)
        {
            return await _areaService.AddArea(request);
        }
    }
}
