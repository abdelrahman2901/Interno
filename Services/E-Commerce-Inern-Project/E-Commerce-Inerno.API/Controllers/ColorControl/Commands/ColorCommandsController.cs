using E_Commerce_Inerno.Core.Features.Color.Command.CreateColorCmd;
using E_Commerce_Inerno.Core.Features.Color.Command.DeleteColorCmd;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ColorControl.Commands
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

        [HttpPost("[action]")]
        [Authorize]
        public async Task<Result<ColorResponse>> CraeteColor(ColorRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("[action]/{ColorID}")]
        [Authorize]
        public async Task<Result<bool>> DeleteColor(Guid ColorID)
        {
            return await _mediator.Send(new DeleteColorRequest(ColorID));
        }
    }
}
