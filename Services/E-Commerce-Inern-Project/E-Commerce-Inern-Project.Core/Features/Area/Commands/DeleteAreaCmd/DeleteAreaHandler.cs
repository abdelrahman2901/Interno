using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.DeleteAreaCmd
{
    public class DeleteAreaHandler : IRequestHandler<DeleteAreaCmd, Result<bool>>
    {
        private readonly IAreaService _areaService;
        public DeleteAreaHandler(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public async Task<Result<bool>> Handle(DeleteAreaCmd request, CancellationToken cancellationToken)
        {
            return await _areaService.DeleteArea(request.AreaID);
        }
    }
}
