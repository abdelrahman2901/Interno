using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Cities.Commands.CreateCityCmd
{
    public record CityRequest(string CityName) : IRequest<Result<bool>>;
}
