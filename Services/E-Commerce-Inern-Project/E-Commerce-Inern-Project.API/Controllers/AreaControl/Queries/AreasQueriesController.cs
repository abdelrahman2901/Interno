using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.Features.Area.Query.GetAllAreasQ;
using E_Commerce_Inern_Project.Core.Features.Area.Query.GetAreaByIDQuery;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.AreaControl.Queries
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

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<IEnumerable<AreaResponse>>> GetAllAreas()
        {
            return await _MediatR.Send(new GetAllAreasQuery());
        }

        [HttpGet("{AreaID}")]
        [AllowAnonymous]
        public async Task<Result<AreaResponse>> GetAllAreas(Guid AreaID)
        {
            return await _MediatR.Send(new GetAreaByIDRequest(AreaID));
        }
    }
}
