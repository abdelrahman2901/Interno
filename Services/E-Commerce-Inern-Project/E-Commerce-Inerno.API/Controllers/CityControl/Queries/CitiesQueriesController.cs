using E_Commerce_Inerno.Core.Features.Cities.Query.GetAllCitiesQ;
using E_Commerce_Inerno.Core.Features.Cities.Query.GetCityByIDQ;
using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inerno.API.Controllers.CityControl.Queries
{
    [AllowAnonymous]
    [Route("api/Cities")]
    [ApiController]
    public class CitiesQueriesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public CitiesQueriesController(IMediator MediatR)
        {
            _MediatR = MediatR;
        }

        [HttpGet("[action]")]
        public async Task<Result<IEnumerable<CityResponse>>> GetAllCities()
        {
            return await _MediatR.Send(new GetCitiesQuery());
        }

        [HttpGet("{CityID}")]
        public async Task<Result<CityResponse>> GetAllCities(Guid CityID)
        {
            return await _MediatR.Send(new GetCityByIDQuery(CityID));
        }
    }
}
