using E_Commerce_Inern_Project.Core.Features.Cities.Commands.CreateCityCmd;
using E_Commerce_Inern_Project.Core.Features.Cities.Commands.DeleteCityCmd;
using E_Commerce_Inern_Project.Core.Features.Cities.Commands.UpdateCityCmd;
using E_Commerce_Interno.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Inern_Project.API.Controllers.CityControl.Commands
{
    [AllowAnonymous]
    [Route("api/Cities")]
    [ApiController]
    public class CitiesCommandsController : ControllerBase
    {
        private readonly IMediator _MediatR;

        public CitiesCommandsController(IMediator MediatR)
        {
            _MediatR = MediatR;
        }

        [HttpPut("[action]")]
        public async Task<Result<bool>> PutCity(CityUpdateRequest CityRequest)
        {
            return await _MediatR.Send(CityRequest);
        }

        [HttpPost("[action]")]
        public async Task<Result<bool>> PostCity(CityRequest CityRequest)
        {
            return await _MediatR.Send(CityRequest);
        }

        [HttpPut("[action]/{id}")]
        public async Task<Result<bool>> DeleteCity(Guid id)
        {
            return await _MediatR.Send(new DeleteCityRequest(id));
        }
    }
}
