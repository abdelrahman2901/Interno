using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Query.GetAllCitiesQ
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, Result<IEnumerable<CityResponse>>>
    {
        private readonly ICityService _cityservice;
        public GetCitiesHandler(ICityService cityservice)
        {
            _cityservice = cityservice;
        }

        public async Task<Result<IEnumerable<CityResponse>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _cityservice.GetAllCities();
        }
    }
}
