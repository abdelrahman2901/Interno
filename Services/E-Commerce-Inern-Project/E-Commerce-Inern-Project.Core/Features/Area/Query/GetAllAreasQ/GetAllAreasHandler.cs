using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Query.GetAllAreasQ
{
    public class GetAllAreasHandler : IRequestHandler<GetAllAreasQuery, Result<IEnumerable<AreaResponse>>>
    {
        private readonly IAreaService _areaService;
        public GetAllAreasHandler(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<Result<IEnumerable<AreaResponse>>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
        {
            return await _areaService.GetAllAreas();
        }
    }
}
