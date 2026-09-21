using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.DeleteSizeCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.SizeControl.Commands
{
    [Route("api/Size")]
    [ApiController]
    public class SizeCommandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizeCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<Result<SizeResponse>> CraeteSize(SizeRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{SizeID}")]
        [Authorize]
        public async Task<Result<bool>> DeleteSize(Guid SizeID)
        {
            return await _mediator.Send(new DeleteSizeCmd(SizeID));
        }
    }
}
