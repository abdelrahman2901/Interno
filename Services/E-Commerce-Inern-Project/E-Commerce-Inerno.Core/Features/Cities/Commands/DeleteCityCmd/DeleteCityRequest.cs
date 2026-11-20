using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Cities.Commands.DeleteCityCmd
{
    public record DeleteCityRequest(Guid CityID) : IRequest<Result<bool>>;
}
