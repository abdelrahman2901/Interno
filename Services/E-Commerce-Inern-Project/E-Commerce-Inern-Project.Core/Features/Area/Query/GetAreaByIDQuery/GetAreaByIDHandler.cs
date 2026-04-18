using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.IAreaServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Query.GetAreaByIDQuery
{
    public class GetAreaByIDHandler : IRequestHandler<GetAreaByIDRequest, Result<AreaResponse>>
    {
        private readonly IAreaService _AreaService;
        public GetAreaByIDHandler(IAreaService AreaService)
        {
            _AreaService = AreaService;
        }
        public async Task<Result<AreaResponse>> Handle(GetAreaByIDRequest request, CancellationToken cancellationToken)
        {
            return await _AreaService.GetArea(request.AreaID);
        }
    }
}
