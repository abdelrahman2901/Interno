using E_Commerce_Interno.Domain.Common;
using MediatR;

namespace E_Commerce_Inerno.Core.Features.Areas.Commands.UpdateAreaCmd
{
    public record AreaUpdateRequest(Guid AreaID, Guid CityID, string AreaName) : IRequest<Result<bool>>;
}
