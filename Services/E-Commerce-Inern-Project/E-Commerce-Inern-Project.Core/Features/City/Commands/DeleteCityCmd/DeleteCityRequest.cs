using E_Commerce_Inern_Project.Core.Common;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.City.Commands.DeleteCityCmd
{
    public record DeleteCityRequest(Guid CityID) : IRequest<Result<bool>>;
}
