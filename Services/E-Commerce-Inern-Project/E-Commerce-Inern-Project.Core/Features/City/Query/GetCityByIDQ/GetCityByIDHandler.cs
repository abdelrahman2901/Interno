using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Query.GetCityByIDQ
{
    public class GetCityByIDHandler : IRequestHandler<GetCityByIDQuery, Result<CityResponse>>
    {
        private readonly ICityService _CityService;
        public GetCityByIDHandler(ICityService cityService)
        {
            _CityService = cityService;
        }

        public async Task<Result<CityResponse>> Handle(GetCityByIDQuery request, CancellationToken cancellationToken)
        {
            return await _CityService.GetCity(request.CityID);
        }
    }
}
