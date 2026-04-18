using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.CreateAreaCmd
{
    public record AreaRequest(Guid CityID, string AreaName) : IRequest<Result<AreaResponse>>;
}
