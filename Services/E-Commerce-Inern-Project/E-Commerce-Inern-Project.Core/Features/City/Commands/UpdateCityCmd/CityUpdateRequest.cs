using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Commands.UpdateCityCmd
{
    public record CityUpdateRequest(Guid CityID, string CityName) : IRequest<Result<bool>>;
}
