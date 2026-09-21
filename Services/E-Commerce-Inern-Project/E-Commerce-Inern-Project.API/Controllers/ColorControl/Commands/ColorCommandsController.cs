using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd;
using E_Commerce_Inern_Project.Core.Features.Color.Command.DeleteColorCmd;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.ColorControl.Commands
{
    [Route("api/Color")]
    [ApiController]
    public class ColorCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<Result<ColorResponse>> CraeteColor(ColorRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{ColorID}")]
        [Authorize]
        public async Task<Result<bool>> DeleteColor(Guid ColorID)
        {
            return await _mediator.Send(new DeleteColorRequest(ColorID));
        }
    }
}
