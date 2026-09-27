using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.CityDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Cities.Query.GetCityByIDQ
{
    public record GetCityByIDQuery(Guid CityID) : IRequest<Result<CityResponse>>;
}
