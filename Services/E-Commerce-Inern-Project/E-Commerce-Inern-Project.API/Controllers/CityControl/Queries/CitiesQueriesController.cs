using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.Features.City.Query.GetAllCitiesQ;
using E_Commerce_Inern_Project.Core.Features.City.Query.GetCityByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.CityControl.Queries
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

        [HttpGet]
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
