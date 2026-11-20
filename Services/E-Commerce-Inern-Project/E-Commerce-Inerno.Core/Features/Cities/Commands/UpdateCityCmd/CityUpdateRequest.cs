using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Cities.Commands.UpdateCityCmd
{
    public record CityUpdateRequest(Guid CityID, string CityName) : IRequest<Result<bool>>;
}
