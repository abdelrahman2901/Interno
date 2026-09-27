using E_Commerce_Inern_Project.Core.Features.Areas.Commands.CreateAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Areas.Commands.DeleteAreaCmd;
using E_Commerce_Inern_Project.Core.Features.Areas.Commands.UpdateAreaCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.AreaControl.Commands
{
    [Route("api/Areas")]
    [ApiController]
    public class AreasCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public AreasCommandsController(IMediator MediatR)
        {
            _MediatR = MediatR;
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> PutArea(AreaUpdateRequest areaRequest)
        {
            return await _MediatR.Send(areaRequest);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<AreaResponse>> PostArea(AreaRequest areaRequest)
        {
            return await _MediatR.Send(areaRequest);
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> DeleteArea(Guid id)
        {
            return await _MediatR.Send(new DeleteAreaCmd(id));
        }
    }
}
