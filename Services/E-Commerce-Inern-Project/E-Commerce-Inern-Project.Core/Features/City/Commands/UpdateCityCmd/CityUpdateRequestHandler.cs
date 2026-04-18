using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd
{
    public class CityUpdateRequestHandler : IRequestHandler<CityUpdateRequest, Result<bool>>
    {
        private readonly ICityService _cityService;
        public CityUpdateRequestHandler(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<Result<bool>> Handle(CityUpdateRequest request, CancellationToken cancellationToken)
        {
            return await _cityService.UpdateCity(request);
        }
    }
}
