using E_Commerce_Inerno.Core.Features.Areas.Query.GetAllAreasQ;
using E_Commerce_Inerno.Core.Features.Areas.Query.GetAreaByIDQuery;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.AreaControl.Queries
{
    [Route("api/Areas")]
    [ApiController]
    public class AreasQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public AreasQueriesController(IMediator MediatR)
        {
            _MediatR = MediatR;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Result<IEnumerable<AreaResponse>>> GetAllAreas()
        {
            return await _MediatR.Send(new GetAllAreasQuery());
        }

        [HttpGet("[action]/{AreaID}")]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<Result<AreaResponse>> GetAreaByID(Guid AreaID)
        {
            return await _MediatR.Send(new GetAreaByIDRequest(AreaID));
        }
    }
}
