using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Commands.DeleteCityCmd
{
    public class DeleteCityHandler : IRequestHandler<DeleteCityRequest, Result<bool>>
    {
        private readonly ICityService _cityService;
        public DeleteCityHandler(ICityService cityService)
        {
            _cityService = cityService;
        }


        async Task<Result<bool>> IRequestHandler<DeleteCityRequest, Result<bool>>.Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            return await _cityService.DeleteCity(request.CityID);
        }
    }
}
