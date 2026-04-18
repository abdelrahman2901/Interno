using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.Features.Color.Command.CreateColorCmd;
using E_Commerce_Inern_Project.Core.Features.Color.Command.DeleteColorCmd;
using E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.ColorControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ColorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<ColorResponse>>> GetAllColors()
        {
            return await _mediator.Send(new GetColorsQuery());
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
