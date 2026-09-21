using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.SizeDTO;
using E_Commerce_Inern_Project.Core.Features.SIze.Query.GetSizesQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.SizeControl.Queries
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<SizeResponse>>> GetAllSizes()
        {
            return await _mediator.Send(new GetSizesQuery());
        }
    }
}
