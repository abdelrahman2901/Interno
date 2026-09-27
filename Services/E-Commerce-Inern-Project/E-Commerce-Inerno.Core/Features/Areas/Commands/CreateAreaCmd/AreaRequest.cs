using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inern_Project.Core.Features.Areas.Commands.CreateAreaCmd
{
    public record AreaRequest(Guid CityID, string AreaName) : IRequest<Result<AreaResponse>>;
}
