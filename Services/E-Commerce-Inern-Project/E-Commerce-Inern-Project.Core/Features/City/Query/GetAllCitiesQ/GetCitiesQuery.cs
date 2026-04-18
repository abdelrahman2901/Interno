using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.CityDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Query.GetAllCitiesQ
{
    public record GetCitiesQuery() : IRequest<Result<IEnumerable<CityResponse>>>;
}
