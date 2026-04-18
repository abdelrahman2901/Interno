using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.UpdateAreaCmd
{
    public class UpdateAreaHandler : IRequestHandler<AreaUpdateRequest, Result<bool>>
    {
        private readonly IAreaService _areaService;
        public UpdateAreaHandler(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<Result<bool>> Handle(AreaUpdateRequest request, CancellationToken cancellationToken)
        {
            return await _areaService.UpdateArea(request);
        }
    }
}
