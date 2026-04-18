
using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.Features.City.Commands.CreateCityCmd;
using E_Commerce_Inern_Project.Core.Features.City.Commands.DeleteCityCmd;
using E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd;
using E_Commerce_Inern_Project.Core.Features.City.Query.GetAllCitiesQ;
using E_Commerce_Inern_Project.Core.Features.City.Query.GetCityByIDQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controller.CityControl
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public CitiesController(IMediator MediatR)
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

        [HttpPut]
        public async Task<Result<bool>> PutCity(  CityUpdateRequest CityRequest)
        {
            return await _MediatR.Send(CityRequest);
        }

        [HttpPost]
        public async Task<Result<bool>> PostCity(CityRequest CityRequest)
        {
            return await _MediatR.Send(CityRequest);
        }

        [HttpPut("{id}")]
        public async Task<Result<bool>> DeleteCity(Guid id)
        {
            return await _MediatR.Send(new DeleteCityRequest(id));
        }


    }
}
