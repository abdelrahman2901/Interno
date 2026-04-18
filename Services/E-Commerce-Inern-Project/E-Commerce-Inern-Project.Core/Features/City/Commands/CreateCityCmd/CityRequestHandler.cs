using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Commands.CreateCityCmd
{
    public class CityRequestHandler : IRequestHandler<CityRequest, Result<bool>>
    {
        private readonly ICityService _cityService;
        public CityRequestHandler(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<Result<bool>> Handle(CityRequest request, CancellationToken cancellationToken)
        {
            return await _cityService.AddCity(request);
        }
    }
}
