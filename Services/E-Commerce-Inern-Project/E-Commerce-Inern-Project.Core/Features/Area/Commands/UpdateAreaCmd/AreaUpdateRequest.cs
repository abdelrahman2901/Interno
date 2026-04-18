using E_Commerce_Inern_Project.Core.Common;
using E_Commerce_Inern_Project.Core.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Area.Commands.UpdateAreaCmd
{
    public record AreaUpdateRequest(Guid AreaID, Guid CityID, string AreaName) : IRequest<Result<bool>>;
}
