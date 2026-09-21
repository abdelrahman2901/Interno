using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.ColorDTO;
using E_Commerce_Inern_Project.Core.Features.Color.Query.GetColorsQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.ColorControl.Queries
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<ColorResponse>>> GetAllColors()
        {
            return await _mediator.Send(new GetColorsQuery());
        }
    }
}
