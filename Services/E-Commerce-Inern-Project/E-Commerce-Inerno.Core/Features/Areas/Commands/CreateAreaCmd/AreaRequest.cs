using E_Commerce_Interno.Domain.Common;
using E_Commerce_Interno.Domain.DTO.AreaDTO;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Areas.Commands.CreateAreaCmd
{
    public record AreaRequest(Guid CityID, string AreaName) : IRequest<Result<AreaResponse>>;
}
