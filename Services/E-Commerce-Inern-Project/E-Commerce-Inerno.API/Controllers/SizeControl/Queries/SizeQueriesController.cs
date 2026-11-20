using E_Commerce_Inerno.Core.Features.SIze.Query.GetSizesQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.SizeDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.SizeControl.Queries
{
    [Route("api/Size")]
    [ApiController]
    public class SizeQueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizeQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<SizeResponse>>> GetAllSizes()
        {
            return await _mediator.Send(new GetSizesQuery());
        }
    }
}
