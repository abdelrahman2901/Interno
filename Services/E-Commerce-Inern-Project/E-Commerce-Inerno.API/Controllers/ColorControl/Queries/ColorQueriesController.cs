using E_Commerce_Inerno.Core.Features.Color.Query.GetColorsQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.ColorDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.ColorControl.Queries
{
    [Route("api/Color")]
    [ApiController]
    public class ColorQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<ColorResponse>>> GetAllColors()
        {
            return await _mediator.Send(new GetColorsQuery());
        }
    }
}
