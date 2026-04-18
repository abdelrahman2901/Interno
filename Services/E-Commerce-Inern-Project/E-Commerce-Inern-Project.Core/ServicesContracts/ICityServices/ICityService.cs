using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using E_Commerce_Inern_Project.Core.Features.City.Commands.CreateCityCmd;
using E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd;

namespace E_Commerce_Inern_Project.Core.ServicesContracts.ICityServices
{
    public interface ICityService 
    {
        public Task<Result<IEnumerable<CityResponse>>> GetAllCities();
        public Task<Result<CityResponse>> GetCity(Guid id);
        public Task<Result<bool>> UpdateCity(CityUpdateRequest City);
        public Task<Result<bool>> AddCity(CityRequest City);
        public Task<Result<bool>> DeleteCity(Guid id);
    }
}
