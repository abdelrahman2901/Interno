using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.CreateSizeCommand;
using E_Commerce_Inern_Project.Core.Features.SIze.Commands.DeleteSizeCommand;
using E_Commerce_Inern_Project.Core.Features.SIze.Query.GetSizesQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.SizeControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SizeController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<SizeResponse>>> GetAllSizes()
        {
            return await _mediator.Send(new GetSizesQuery());
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
