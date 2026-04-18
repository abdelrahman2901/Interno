using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.DeleteAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Area.Commands.UpdateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Area.Query.GetAllAreasQ;
using E_Commerce_Inern_Project.Core.Features.Area.Query.GetAreaByIDQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.AreaControl
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController] 
    public class AreasController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public AreasController(IMediator MediatR)
        {
            _MediatR = MediatR;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<AreaResponse>>> GetAllAreas()
        {
            return await _MediatR.Send(new GetAllAreasQuery());
        }
        [HttpGet("{AreaID}")]
        public async Task<Result<AreaResponse>> GetAllAreas(Guid AreaID)
        {
            return await _MediatR.Send(new GetAreaByIDRequest(AreaID));
        }

        [HttpPut]
        public async Task<Result<bool>> PutArea( AreaUpdateRequest areaRequest)
        {
            return await _MediatR.Send(areaRequest);

        }

        [HttpPost]
        public async Task<Result<AreaResponse>> PostArea(AreaRequest areaRequest)
        {
            return await _MediatR.Send(areaRequest);
        }

        [HttpPut("{id}")]
        public async Task<Result<bool>> DeleteArea(Guid id)
        {
            return await _MediatR.Send(new DeleteAreaCmd(id));

        }

    }
}
